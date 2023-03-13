using System.Collections.Generic;
using Portfolio.Models;
using System.Threading.Tasks;
using System.Linq;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Azure.Storage.Blobs.Models;

namespace Portfolio
{
    public class StorageConnector
    {
        private readonly BlobContainerClient blobClientProjects;
        private readonly CloudStorageAccount cloudStorageAccount;
        private readonly CloudTableClient tableClient;

        public StorageConnector(string connectionString)
        {
            blobClientProjects = new BlobContainerClient(connectionString, "projects");
            cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = cloudStorageAccount.CreateCloudTableClient();
        }

        public async Task<IEnumerable<EventEntity>> LoadEvents()
        {
            var eventsTable = tableClient.GetTableReference("Events");
            IEnumerable<EventEntity> events = await eventsTable.ExecuteQuerySegmentedAsync<EventEntity>(new TableQuery<EventEntity>(), null);
            return events.OrderBy(x => x.Date);
        }

        public async Task<IEnumerable<SkillEntity>> LoadSkills()
        {
            var skillsTable = tableClient.GetTableReference("Skills");
            IEnumerable<SkillEntity> skills = await skillsTable.ExecuteQuerySegmentedAsync<SkillEntity>(new TableQuery<SkillEntity>(), null);
            return skills;
        }

        public async Task<IEnumerable<SkillGroupEntity>> LoadSkillGroups()
        {
            IEnumerable<SkillEntity> skills = await LoadSkills();

            var skillGroupsTable = tableClient.GetTableReference("SkillGroups");
            IEnumerable<SkillGroupEntity> skillGroups = await skillGroupsTable.ExecuteQuerySegmentedAsync<SkillGroupEntity>(new TableQuery<SkillGroupEntity>(), null);
            skillGroups.ToList().ForEach(group => group.skills.AddRange(skills.Where(skill => skill.SkillGroup == group.Id)));

            return skillGroups;
        }

        public async Task<IEnumerable<ProjectEntity>> LoadProjects()
        {
            var projectsTable = tableClient.GetTableReference("Projects");
            IEnumerable<ProjectEntity> projects = await projectsTable.ExecuteQuerySegmentedAsync<ProjectEntity>(new TableQuery<ProjectEntity>(), null);

            var projectLinkTable = tableClient.GetTableReference("ProjectLinks");
            IEnumerable<ProjectLinkEntity> projectLinks = await projectLinkTable.ExecuteQuerySegmentedAsync<ProjectLinkEntity>(new TableQuery<ProjectLinkEntity>(), null);

            var projectVideosTable = tableClient.GetTableReference("ProjectVideos");
            IEnumerable<ProjectVideoEntity> projectVideos = await projectVideosTable.ExecuteQuerySegmentedAsync<ProjectVideoEntity>(new TableQuery<ProjectVideoEntity>(), null);

            var projectSkillTable = tableClient.GetTableReference("ProjectSkills");
            IEnumerable<ProjectSkillEntity> projectSkills = await projectSkillTable.ExecuteQuerySegmentedAsync<ProjectSkillEntity>(new TableQuery<ProjectSkillEntity>(), null);

            IEnumerable<SkillEntity> skills = await LoadSkills();

            var tasks = projects.ToList().Select(project => {
                project.links.AddRange(projectLinks.Where(link => link.ProjectId == project.Id));
                project.videos.AddRange(projectVideos.Where(video => video.ProjectId == project.Id));
                project.skills.AddRange(from skill in skills
                                        join ps in projectSkills on skill.Id equals ps.SkillId
                                        where ps.ProjectId == project.Id
                                        select skill);

                for (int i = 0; i < project.ImageNumber; i++)
                {
                    project.images.Add($"{blobClientProjects.Uri.AbsoluteUri}/{project.ImageFolder}/{i}.webp");
                }
                return Task.CompletedTask;
            });
            await Task.WhenAll(tasks);

            

            return projects.OrderByDescending(x => x.ReleaseDate);
        }
    }
}
