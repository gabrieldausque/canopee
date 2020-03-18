// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const path = require('path');
const {remote, ipcRenderer} = require('electron');
const {app, Menu, Tray} = remote;

function addTrayIcon(fullpathToIcon, menuItem) {
    tray = new Tray(fullpathToIcon);
    console.log(menuItem);
    tray.setToolTip('Canopee Agent');
}

let tray = null;
ipcRenderer.on('create-tray', (arg) => {
    console.log(arg);
    alert("create tray asked")
});
//app.whenReady().then(addTrayIcon);
