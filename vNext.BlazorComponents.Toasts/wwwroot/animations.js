var vNext;
(function (vNext) {
    var BlazorComponents;
    (function (BlazorComponents) {
        var Toasts;
        (function (Toasts) {
            function addToastAnimations(container, options) {
                if (!options) {
                    options = {
                        duration: 250,
                        easing: 'ease-out',
                    };
                }
                let oldFlexItemsInfo = getFlexItemsInfo(container);
                // Callback function to execute when children are added or removed
                const callback = function () {
                    const newFlexItemsInfo = getFlexItemsInfo(container);
                    animateFlexItems(oldFlexItemsInfo, newFlexItemsInfo);
                    oldFlexItemsInfo = newFlexItemsInfo;
                };
                const observer = new MutationObserver(callback);
                observer.observe(container, { childList: true });
                function getFlexItemsInfo(container) {
                    return Array.from(container.children).map((item) => {
                        const rect = item.getBoundingClientRect();
                        return {
                            element: item,
                            x: rect.left,
                            y: rect.top,
                            width: rect.right - rect.left,
                            height: rect.bottom - rect.top,
                        };
                    });
                }
                function animateFlexItems(oldFlexItemsInfo, newFlexItemsInfo) {
                    for (const newFlexItemInfo of newFlexItemsInfo) {
                        const oldFlexItemInfo = oldFlexItemsInfo.find(e => e.element == newFlexItemInfo.element);
                        if (!oldFlexItemInfo) {
                            continue;
                        }
                        const translateX = oldFlexItemInfo.x - newFlexItemInfo.x;
                        const translateY = oldFlexItemInfo.y - newFlexItemInfo.y;
                        const scaleX = oldFlexItemInfo.width / newFlexItemInfo.width;
                        const scaleY = oldFlexItemInfo.height / newFlexItemInfo.height;
                        newFlexItemInfo.element.animate([
                            {
                                transform: `translate(${translateX}px, ${translateY}px) scale(${scaleX}, ${scaleY})`,
                            },
                            { transform: 'none' },
                        ], options);
                    }
                }
            }
            Toasts.addToastAnimations = addToastAnimations;
        })(Toasts = BlazorComponents.Toasts || (BlazorComponents.Toasts = {}));
    })(BlazorComponents = vNext.BlazorComponents || (vNext.BlazorComponents = {}));
})(vNext || (vNext = {}));
//# sourceMappingURL=animations.js.map