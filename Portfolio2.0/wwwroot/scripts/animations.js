import anime from '../libraries/animejs/anime.es.js';

anime({
    targets: 'header img',
    translateY: ['100vh', '0vh'],
    rotateZ: '-3turn',
    easing: 'spring(5, 80, 36, 0)'
});

anime({
    targets: '#photo',
    translateX: ['100vw', '0vw'],
    rotateZ: '-3turn',
    easing: 'spring(5, 80, 36, 0)',
});

anime({
    targets: 'header a',
    opacity: ['0%', '100%'],
    easing: 'easeInBounce',
    delay: 1000
});

anime({
    targets: '.iaro, #title h1',
    opacity: ['0%', '100%'],
    easing: 'easeInBounce',
    delay: 1000
});

anime({
    targets: '#title h6, #mynetwork, .projectPack, .fullPath, .skillFilters',
    opacity: ['0%', '100%'],
    easing: 'easeInBounce',
    delay: 2000
});

anime({
    targets: '.loading_spinner',
    opacity: ['0%', '25%'],
    easing: 'easeInBounce',
    delay: 2000
});