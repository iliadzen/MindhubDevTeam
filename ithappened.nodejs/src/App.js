import {BrowserRouter, Switch, Route, Redirect} from "react-router-dom";
import UserSignUp from "./User/UserSignUp";
import UserSignIn from "./User/UserSignIn";
import UserApp from "./User/UserApp";
import UserSignOut from "./User/UserSignOut";
import UserEdit from "./User/UserEdit";
import Trackers from "./Tracker/Trackers";
import TrackerDetail from "./Tracker/TrackerDetail";
import TrackerDelete from "./Tracker/TrackerDelete";
import TrackerEdit from "./Tracker/TrackerEdit";

const welcomeText = () => (
    <div>
        <h1>WARNING: Lol Exception</h1>
        <ul>
            <li><a href="users/signup">Sign Up</a></li>
            <li><a href="users/signin">Sign In</a></li>
            <li><a href="users/signout">Sign Out</a></li>
            <li><a href="users/self">Profile</a></li>
            <hr></hr>
            <li><a href="trackers/create">Create Tracker</a></li>
            <li><a href="trackers">All Trackers</a></li>
        </ul>
    </div>
);

const App = () => {
    return (
        <BrowserRouter>
            <Switch>
                <Route path="/users/signup">
                    {localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserSignUp />}
                </Route>
                <Route path="/users/signin">
                    {localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserSignIn />}
                </Route>
                <Route path="/users/self">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/users/signin"/> : <UserApp />}
                </Route>
                <Route path="/users/signout">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserSignOut />}
                </Route>
                <Route path="/users/edit">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <UserEdit />}
                </Route>

                <Route path="/trackers/:trackerId/edit">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <TrackerEdit />}
                </Route>
                <Route path="/trackers/:trackerId/delete">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <TrackerDelete />}
                </Route>
                <Route path="/trackers/:trackerId">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <TrackerDetail />}
                </Route>
                <Route path="/trackers">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <Trackers />}
                </Route>

                <Route path="/" component={welcomeText}/>
            </Switch>
        </BrowserRouter>
    )
}

export default App