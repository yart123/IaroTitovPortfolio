import anime from '../libraries/animejs/anime.es.js';

window.addEventListener('load', function () {

	var duration = 300;
	function slideClick() {
		if (this.classList.contains("right") || this.classList.contains("left")) {
			var id = '#'+this.parentNode.id;

			anime({
					targets: id + ' .slide',
					opacity: [1, 1],
					easing: 'linear',
					duration: duration,
				});

			if (this.classList.contains("right")) {
				anime({
						targets: id + ' .selected',
						scale: ['1', '0.9'],
						opacity: [1, 0.5, 1],
						translateX: '-9vw',
						zIndex: [1, 0, 0],
						easing: 'linear',
						duration: duration,
					});
				anime({
						targets: id + ' .right',
						scale: ['0.9', '1'],
						opacity: [1, 0.5, 1],
						translateX: ['9vw', '0'],
						zIndex: [0, 1, 1],
						easing: 'linear',
						duration: duration,
					});
				anime({
						targets: id + ' .left',
						opacity: [1, 0],
						easing: 'linear',
						duration: duration,
					});
			}
			else {
				anime({
						targets: id + ' .selected',
						scale: ['1', '0.9'],
						opacity: [1, 0.5, 1],
						translateX: '9vw',
						zIndex: [1, 0, 0],
						easing: 'linear',
						duration: duration,
					});
				anime({
						targets: id + ' .left',
						scale: ['0.9', '1'],
						opacity: [1, 0.5, 1],
						translateX: ['-9vw', '0'],
						zIndex: [0, 1, 1],
						easing: 'linear',
						duration: duration,
					});
				anime({
						targets: id + ' .right',
						opacity: [1, 0],
						easing: 'linear',
						duration: duration,
					});
			}
			setTimeout(() => { assignClasses(this) }, 300);
		}
	}

	function assignClasses(node) {
		if (node.previousElementSibling != null) {
			node.previousElementSibling.className = "slide left";

			if (node.previousElementSibling.previousElementSibling != null) {
				node.previousElementSibling.previousElementSibling.className = "slide";
				node.previousElementSibling.previousElementSibling.removeAttribute('style');
			}
		}

		node.className = " slide selected";
		if (node.nextElementSibling != null) {
			node.nextElementSibling.className = "slide right";

			if (node.nextElementSibling.nextElementSibling != null) {
				node.nextElementSibling.nextElementSibling.className = "slide";
				node.nextElementSibling.nextElementSibling.removeAttribute('style');
			}
		}
	}

	var projects = document.querySelectorAll(".project");

	for (var i = 0; i < projects.length; i++) {
		var slides = projects[i].querySelectorAll(".slide");
		slides[0].classList.add("selected");
		slides[1].classList.add("right");

		for (var j = 0; j < slides.length; j++) {
			slides[j].addEventListener("click", slideClick);
		}
	}
});