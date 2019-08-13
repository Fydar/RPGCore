(function () {
    const q = (selector) => document.querySelector(selector);

    const gameContainer = q('#gameContainer');

    gameContainer.style.width = '100%';
    gameContainer.style.height = '100%';

    let canvasElement = null;

    const getCanvasFromMutationsList = (mutationsList) => {
        for (let mutationItem of mutationsList) {
            for (let addedNode of mutationItem.addedNodes) {
                if (addedNode.id === '#canvas') {
                    return addedNode;
                }
            }
        }
        return null;
    }

    const setDimensions = () => {
        gameContainer.style.position = 'absolute';
        canvasElement.style.display = 'none';
        var windowWidth = parseInt(window.getComputedStyle(gameContainer).width, 10);
        var windowHeight = parseInt(window.getComputedStyle(gameContainer).height, 10);
        canvasElement.style.display = '';

        if (canvasElement) {
            canvasElement.setAttribute('width', windowWidth);
            canvasElement.setAttribute('height', windowHeight);
        }
    }

    window.setDimensions = setDimensions;

    const registerCanvasWatcher = () => {
        let debounceTimeout = null;
        const debouncedSetDimensions = () => {
            if (debounceTimeout !== null) {
                clearTimeout(debounceTimeout);
            }
            debounceTimeout = setTimeout(setDimensions, 200);
        }
        window.addEventListener('resize', debouncedSetDimensions, false);
        setDimensions();
    }

    window.UnityLoader.Error.handler = function () { }

    const i = 0;
    new MutationObserver(function (mutationsList) {
        const canvas = getCanvasFromMutationsList(mutationsList)
        if (canvas) {
            canvasElement = canvas;
            registerCanvasWatcher();

            new MutationObserver(function (attributesMutation) {
                this.disconnect();
                setTimeout(setDimensions, 1)
            }).observe(canvas, { attributes: true });

            this.disconnect();
        }
    }).observe(gameContainer, { childList: true });

})();
