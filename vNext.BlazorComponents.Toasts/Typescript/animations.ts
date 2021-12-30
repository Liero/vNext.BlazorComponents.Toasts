namespace vNext.BlazorComponents.Toasts {

    /* https://codepen.io/MauriciAbad/pen/yLbrpey */
    interface FlexItemInfo {
        element: Element

        x: number
        y: number
        width: number
        height: number
    }

    export function addToastAnimations(container: HTMLElement, options?: number | KeyframeAnimationOptions) {
        if (!options) {
            options = {
                duration: 250,
                easing: 'ease-out',
            }
        }
        let oldFlexItemsInfo: FlexItemInfo[] = getFlexItemsInfo(container);

        // Callback function to execute when children are added or removed
        const callback = function () {
            const newFlexItemsInfo = getFlexItemsInfo(container);
            animateFlexItems(oldFlexItemsInfo, newFlexItemsInfo);
            oldFlexItemsInfo = newFlexItemsInfo;
        };

        const observer = new MutationObserver(callback);
        observer.observe(container, { childList: true });

        function getFlexItemsInfo(container: Element): FlexItemInfo[] {
            return Array.from(container.children).map((item) => {
                const rect = item.getBoundingClientRect()
                return {
                    element: item,
                    x: rect.left,
                    y: rect.top,
                    width: rect.right - rect.left,
                    height: rect.bottom - rect.top,
                }
            })
        }

        function animateFlexItems(oldFlexItemsInfo: FlexItemInfo[], newFlexItemsInfo: FlexItemInfo[]): void {
            for (const newFlexItemInfo of newFlexItemsInfo) {
                const oldFlexItemInfo = oldFlexItemsInfo.find(e => e.element == newFlexItemInfo.element);


                if (!oldFlexItemInfo) {
                    continue;
                }

                const translateX = oldFlexItemInfo.x - newFlexItemInfo.x
                const translateY = oldFlexItemInfo.y - newFlexItemInfo.y
                const scaleX = oldFlexItemInfo.width / newFlexItemInfo.width
                const scaleY = oldFlexItemInfo.height / newFlexItemInfo.height


                newFlexItemInfo.element.animate(
                    [
                        {
                            transform: `translate(${translateX}px, ${translateY}px) scale(${scaleX}, ${scaleY})`,
                        },
                        { transform: 'none' },
                    ],
                    options
                )
            }
        }
    }
}
