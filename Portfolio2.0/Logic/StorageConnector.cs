using System.Collections.Generic;
using Portfolio.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Portfolio
{
    public class StorageConnector
    {
        private readonly CloudStorageAccount storageAccount;
        private readonly CloudTableClient tableClient;
        private readonly CloudBlobClient blobClient;

        public StorageConnector(string connectionString)
        {
            storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<IEnumerable<EventEntity>> LoadEvents()
        {
            var eventsTable = tableClient.GetTableReference("Events");
            IEnumerable<EventEntity> events = await eventsTable.ExecuteQuerySegmentedAsync(new TableQuery<EventEntity>(), null);
            return events;
        }

        public async Task<IEnumerable<SkillEntity>> LoadSkills()
        {
            var skillsTable = tableClient.GetTableReference("Skills");
            IEnumerable<SkillEntity> skills = await skillsTable.ExecuteQuerySegmentedAsync(new TableQuery<SkillEntity>(), null);
            return skills;
        }

        public async Task<IEnumerable<SkillGroupEntity>> LoadSkillGroups()
        {
            IEnumerable<SkillEntity> skills = await LoadSkills();

            var skillGroupsTable = tableClient.GetTableReference("SkillGroups");
            IEnumerable<SkillGroupEntity> skillGroups = await skillGroupsTable.ExecuteQuerySegmentedAsync(new TableQuery<SkillGroupEntity>(), null);

            skillGroups.ToList().ForEach(group => group.skills.AddRange(skills.Where(skill => skill.SkillGroup == group.Id)));

            return skillGroups;
        }

        public async Task<IEnumerable<ProjectEntity>> LoadProjects()
        {
            var projectsTable = tableClient.GetTableReference("Projects");
            IEnumerable<ProjectEntity> projects = await projectsTable.ExecuteQuerySegmentedAsync(new TableQuery<ProjectEntity>(), null);

            var projectLinkTable = tableClient.GetTableReference("ProjectLinks");
            IEnumerable<ProjectLinkEntity> projectLinks = await projectLinkTable.ExecuteQuerySegmentedAsync(new TableQuery<ProjectLinkEntity>(), null);

            var projectVideosTable = tableClient.GetTableReference("ProjectVideos");
            IEnumerable<ProjectVideoEntity> projectVideos = await projectVideosTable.ExecuteQuerySegmentedAsync(new TableQuery<ProjectVideoEntity>(), null);

            var projectSkillTable = tableClient.GetTableReference("ProjectSkills");
            IEnumerable<ProjectSkillEntity> projectSkills = await projectSkillTable.ExecuteQuerySegmentedAsync(new TableQuery<ProjectSkillEntity>(), null);

            IEnumerable<SkillEntity> skills = await LoadSkills();

            var tasks = projects.ToList().Select(async project => 
            {
                project.links.AddRange(projectLinks.Where(link => link.ProjectId == project.Id));
                project.videos.AddRange(projectVideos.Where(video => video.ProjectId == project.Id));
                project.skills.AddRange( from skill in skills
                                         join ps in projectSkills on skill.Id equals ps.SkillId
                                         where ps.ProjectId == project.Id
                                         select skill );

                CloudBlobContainer imageFolder = blobClient.GetContainerReference("projects");
                var directory = imageFolder.GetDirectoryReference(project.ImageFolder);
                var imageList = await directory.ListBlobsSegmentedAsync(null);
                project.images = imageList.Results.OfType<CloudBlockBlob>().Select(b => b.Uri.ToString()).ToList();
            });
            await Task.WhenAll(tasks);

            return projects;
        }
    }
}
