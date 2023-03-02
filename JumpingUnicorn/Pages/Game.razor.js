//Function to change the bottom of the avatar, to make it jump.
export function ChangeAvatarBottom(height) {
    //PUT SHIT HERE
     document.getElementById('GameAvatar').style.bottom = height + 'vh'
}

//Function to hide og make the obstacles visible
export function ChangeObstacleVisibility(visibility, id) {
    document.getElementById(id).style.visibility = visibility
}

//function to move the obstacle, used for moving the obstacles from left to right
export function ChangeObstaclePosition(position, id) {
    document.getElementById(id).style.right = position
}

export function ChangeAvatarBottomx() {
    let avatarBottom = parseFloat(window.getComputedStyle(document.getElementById('GameAvatar')).getPropertyValue('bottom'));
    let height = 0;
    while (avatarBottom < 30) {
        height = avatarBottom + (8 / avatarBottom);
        document.getElementById('GameAvatar').style.bottom = height + 'vh';
        setTimeout(10);
    }
}

//EventListeners for keyboard and mouse input. Inputs are sent to the code behind method ReturnKeyStroke()
window.instantiateListeners = (dotNetHelper) => {
    //This is the listener for keyboard inputs.
    addEventListener("keydown", ( event ) => {
        dotNetHelper.invokeMethodAsync('ReturnKeystroke', 'Keyboard:' + event.keyCode);
    });
    //This is the listener for mouse inputs.
    addEventListener("mousedown", (event) => {
        dotNetHelper.invokeMethodAsync('ReturnKeystroke', 'Mouse:' + event.buttons);
    });
}