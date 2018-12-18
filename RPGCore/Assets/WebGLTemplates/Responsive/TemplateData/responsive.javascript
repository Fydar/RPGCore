(function () {
    const q = (selector) => document.querySelector(selector);

    const gameContainer = q('#gameContainer');

    const initialDimensions = { width: parseInt(gameContainer.style.width, 10), height: parseInt(gameContainer.style.height, 10) };
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
        var scale = Math.min(windowWidth / initialDimensions.width, windowHeight / initialDimensions.height);
        canvasElement.style.display = '';

        var fitW = Math.round(initialDimensions.width * scale * 100) / 100;
        var fitH = Math.round(initialDimensions.height * scale * 100) / 100;

        if (canvasElement) {
            canvasElement.setAttribute('width', fitW);
            canvasElement.setAttribute('height', fitH);
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
                q('.simmer').classList.add('hide');
            }).observe(canvas, { attributes: true });

            this.disconnect();
        }
    }).observe(gameContainer, { childList: true });

})();