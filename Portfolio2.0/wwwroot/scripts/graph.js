import anime from '../libraries/animejs/anime.es.js';

window.addEventListener('load', function ()
{
    // Create all nodes and edges
    var skills = JSON.parse(iaroSkillsJson);

    var nodes = new vis.DataSet([{ id: "root", label: 'Skills', mass: 1, heightConstraint: 200, widthConstraint: 200, font: { size: 50 },}]);
    var edges = new vis.DataSet([]);
    var skillGroupsNodes = [];
    var skillGroupsEdges = [];
    var skillNodes = [];
    var skillEdges = [];

    for (var i = 0; i < skills.length; i++)
    {
        var skillGroup = skills[i];
        skillGroupsNodes.push({ id: "group"+skillGroup.Id, label: skillGroup.SkillGroupName, mass: 1, heightConstraint: 150, widthConstraint: 150, font: { size: 25 }});
        skillGroupsEdges.push({ from: "root", to: "group" + skillGroup.Id });

        skillNodes[skillGroup.Id] = [];
        skillEdges[skillGroup.Id] = [];
        for (var j = 0; j < skillGroup.skills.length; j++)
        {
            var skill = skillGroup.skills[j];
            var size = 25 + 100 * skill.Proficiency;
            var fontSize = 10 + 10 * skill.Proficiency - (skill.SkillName.length - 15) * 0.5;
            skillNodes[skillGroup.Id].push({ id: "skill" + skill.Id, label: skill.SkillName, mass: 1, heightConstraint: size, widthConstraint: size, margin: 3, font: { size: fontSize } });
            skillEdges[skillGroup.Id].push({ from: "skill" + skill.Id, to: "group" + skillGroup.Id, length : 0, width: 30});
        }
    }

    //On click logic
    var network;

    var inClick = false;
    function nodeClick(id)
    {
        if (inClick) return;
        inClick = true;

        if (id == "root" && network.getConnectedEdges(id).length == 0) {
            nodes.add(skillGroupsNodes);
            edges.add(skillGroupsEdges);
            network.selectNodes(network.getConnectedNodes(id));
        }
        else
            if (id.includes("group") && network.getConnectedEdges(id).length == 1) {
                var index = parseInt(id.substring(5));
                nodes.add(skillNodes[index]);
                edges.add(skillEdges[index]);
                var skillIds = network.getConnectedNodes("root");
                var allIds = [];
                for (var i = 0; i < skillIds.length; i++) {
                    allIds = allIds.concat(network.getConnectedNodes(skillIds[i]));
                }
                allIds = allIds.concat(skillIds);
                network.selectNodes(allIds);
            }

        network.unselectAll();

        inClick = false;
    }

    // create a network
    var container = document.getElementById('mynetwork');
    var data = {
        nodes: nodes,
        edges: edges
    };
    var options = {
        edges: {
            color: '#ffd200',
            width: 50,
            length: 100,
        },
        nodes: {
            borderWidth: 0,
            color: {
                background: '#ffd200',
                highlight: '#ffd200',
            },
            chosen: {
                label: false,
            },
            value: 1,
            heightConstraint: 100,
            widthConstraint: 100,
            margin: 10,
            shape: 'circle',
            font: {
                face: 'Cambria',
                size: 35,
                color: '#111111'
            },
        },
        height: '100%',
        width: '100%',
        interaction: {
            dragView: false,
            selectable: true,
            zoomView: false,
        },
        physics: {
            minVelocity: 0.05,
            maxVelocity: 5,
            barnesHut : {
                theta: 0.75,
                gravitationalConstant: -5000,
                centralGravity: 0,
            }
        },
    };
    network = new vis.Network(container, data, options);

    network.on('click', function (properties)
    {
        if (properties.nodes.length > 0)
            nodeClick(properties.nodes[0]);
    }); 

    function remove(array, value) {
        const index = array.findIndex(obj => obj === value);
        return index >= 0 ? [
            ...array.slice(0, index),
            ...array.slice(index + 1)
        ] : array;
    }

    var rippleAnime = anime({
        targets: '#ripple',
        opacity: ['0%', '100%'],
        direction: 'alternate',
        easing: 'linear',
        autoplay: 'false',
    });

    var selectedNode = "root";
    function showHelper() {
        var selection = network.getConnectedNodes("root");
        if (selection.length > 0) {
            var allNodes = [];
            for (var i = 0; i < selection.length; i++) {
                var leaves = network.getConnectedNodes(selection[i]);
                if (leaves.length > 1) {
                    leaves = remove(leaves, "root");
                    allNodes = allNodes.concat(leaves);
                    selection = remove(selection, selection[i--]);
                }
            }
            allNodes = allNodes.concat(selection);
            selectedNode = allNodes[Math.floor(Math.random() * allNodes.length)];
            if (selectedNode.includes("skill"))
                ripple.style.transform = "scale(0.5) translate(-200px, -200px)";
            else
                ripple.style.transform = "scale(1) translate(-100px, -100px)";
        }

        rippleAnime.restart();
        setTimeout(showHelper, 3000 + Math.random() * 2000);
    }

    rippleAnime.pause();
    setTimeout(showHelper, 5000);
    var ripple = document.getElementById('ripple');

    network.on('afterDrawing', function ()
    {
        var rootPos = network.canvasToDOM(network.getPosition(selectedNode));
        ripple.style.top = rootPos.y;
        ripple.style.left = rootPos.x;
    }); 
});