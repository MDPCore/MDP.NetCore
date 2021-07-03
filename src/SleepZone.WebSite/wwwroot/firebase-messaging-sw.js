importScripts("https://www.gstatic.com/firebasejs/8.2.9/firebase-app.js");
importScripts("https://www.gstatic.com/firebasejs/8.2.9/firebase-messaging.js");
importScripts("firebase-init.js");

// Messaging
const messaging = firebase.messaging();
const messagingWorker = new BroadcastChannel("firebase-messaging-worker");

// OnBackgroundMessage
messaging.onBackgroundMessage(function (payload) {
    messagingWorker.postMessage(payload);
});