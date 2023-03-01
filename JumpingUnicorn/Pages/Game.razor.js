 export function ChangeAvatarBottom(height){
    //PUT SHIT HERE
     document.getElementById('GameAvatar').style.bottom = height + 'vh'
}

export function ChangeAvatarBottomx() {
    //PUT SHIT HERE
    //let height = document.getElementById('GameAvatar');
    let avatarBottom = parseFloat(window.getComputedStyle(document.getElementById('GameAvatar')).getPropertyValue('bottom'));
    let height = 0;
    while (avatarBottom < 30) {
        height = avatarBottom + (8 / avatarBottom);
        document.getElementById('GameAvatar').style.bottom = height + 'vh';
        setTimeout(10);
    }
}

window.instantiateListeners = (dotNetHelper) => {
    addEventListener("keydown", ( event ) => {
        dotNetHelper.invokeMethodAsync('ReturnKeystroke', 'Keyboard:' + event.keyCode);
    });

    addEventListener("mousedown", (event) => {
        dotNetHelper.invokeMethodAsync('ReturnKeystroke', 'Mouse:' + event.buttons);
    });
}