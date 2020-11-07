import axios from "axios";
const apiBaseUrl = "http://localhost:5000";
const instance = axios.create({
    baseURL: apiBaseUrl
});

const authorizedRequestConfig = { headers: { Authorization: "Bearer "+localStorage.getItem("accessToken") }}

// User
export const userSignUp = (username, password) => instance.post("/users/signup", {username: username, password: password}).then(result => result.data);
export const userSignIn = (username, password) => instance.post("/users/signin", {username: username, password: password}).then(result => result.data);
export const userEdit = (username, password) => instance.put("/users/self", {username: username, password: password}, authorizedRequestConfig).then(result => result.data);
export const getSelf = () => instance.get("/users/self", authorizedRequestConfig).then(result => result.data);

// Tracker
export const getTrackers = () => instance.get("/trackers", authorizedRequestConfig).then(result => result.data);
export const getTracker = (trackerId) => instance.get(`/trackers/${trackerId}`, authorizedRequestConfig).then(result => result.data);
export const editTracker = (trackerName, trackerId) => instance.put(`/trackers/${trackerId}`, {title: trackerName}, authorizedRequestConfig).then(result => result.data);
export const deleteTracker = (trackerId) => instance.delete(`/trackers/${trackerId}`, authorizedRequestConfig).then(result => result.data);
export const createTracker = (trackerTitle, trackerCustomizations) => instance.post(`/trackers`, {title: trackerTitle, customizations: trackerCustomizations}, authorizedRequestConfig).then(result => result.data);

// Event
export const createEvent = (trackerId, data) => instance.post(`/trackers/${trackerId}/events`, data, authorizedRequestConfig).then(result => result.data);
export const editEvent = (trackerId, eventId, eventTitle) => instance.put(`/trackers/${trackerId}/events/${eventId}`, {title: eventTitle}, authorizedRequestConfig).then(result => result.data);
export const deleteEvent = (trackerId, eventId) => instance.delete(`/trackers/${trackerId}/events/${eventId}`, authorizedRequestConfig).then(result => result.data);
export const getEvents = (trackerId) => instance.get(`/trackers/${trackerId}/events`, authorizedRequestConfig).then(result => result.data);
export const getEvent = (trackerId, eventId) => instance.get(`/trackers/${trackerId}/events/${eventId}`, authorizedRequestConfig).then(result => result.data);

// Stats
export const getStats = () => instance.get("/stats", authorizedRequestConfig).then(result => result.data);
export const getStatsForTracker = (trackerId) => instance.get(`/stats/${trackerId}`, authorizedRequestConfig).then(result => result.data);
