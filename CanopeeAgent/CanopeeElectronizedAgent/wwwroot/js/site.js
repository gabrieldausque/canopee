// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const path = require('path');
const {remote} = require('electron');
const {app, Menu, Tray} = remote;
console.log(__dirname);
function addTrayIcon() {
    console.log(path.resolve('../../bin/wwwroot/images/NotifyIcon.png'));
    tray = new Tray(path.resolve('../../bin/wwwroot/images/NotifyIcon.png'));
    const contextMenu = Menu.buildFromTemplate([
        {label: 'Stop Canopee Agent', type: 'normal'}
    ])
    tray.setToolTip('Canopee Agent');
    tray.setContextMenu(contextMenu);
}

let tray = null;
console.log('before app.on');
app.whenReady().then(addTrayIcon);
