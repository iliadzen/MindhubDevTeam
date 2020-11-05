import {BrowserRouter, Switch, Route, Redirect} from "react-router-dom";
import UserSignUp from "./User/UserSignUp";
import UserSignIn from "./User/UserSignIn";
import UserApp from "./User/UserApp";
import UserSignOut from "./User/UserSignOut";
import UserEdit from "./User/UserEdit";
import Trackers from "./Tracker/Trackers";
import TrackerDetail from "./Tracker/TrackerDetail";
import TrackerEdit from "./Tracker/TrackerEdit";
import TrackerCreate from "./Tracker/TrackerCreate";
import LinkButton from "./Components/Common/LinkButton"

const welcomeText = () => (
    <div>
        <h1>It Happened!</h1>
        <h3>Come as you are. Again.</h3>
        <LinkButton url={'users/signup'} text="Sign Up"/><br></br>
        <LinkButton url={'users/signin'} text="Sign In"/><br></br>
        <LinkButton url={'users/signout'} text="Sign Out"/><br></br>
        <LinkButton url={'users/self'} text="Profile"/><br></br>
        <hr></hr>
        <LinkButton url={'trackers/create'} color="secondary" text="Create Tracker"/><br></br>
        <LinkButton url={'trackers'} color="secondary" text="All Trackers"/><br></br>
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

                <Route path="/trackers/create">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <TrackerCreate />}
                </Route>
                <Route path="/trackers/:trackerId/edit">
                    {!localStorage.getItem("accessToken") ? <Redirect to="/"/> : <TrackerEdit />}
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