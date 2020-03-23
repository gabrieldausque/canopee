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
app.whenReady().then(() => {
    ipcRenderer.on('canopee-logs', (event, args) => {
       var logs = jQuery('#canopee-logs');
       if(logs.length >= 1) {
           const lastElement = jQuery(args).appendTo('#canopee-logs');
           logs.animate({scrollTop: lastElement.offset().top}, 10);
       }
    });
});
