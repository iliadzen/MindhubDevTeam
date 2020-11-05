import React, { useState } from 'react';
import {Link, useHistory} from 'react-router-dom';
import Avatar from '@material-ui/core/Avatar';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import TextField from '@material-ui/core/TextField';
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Grid from '@material-ui/core/Grid';
import Box from '@material-ui/core/Box';
import LockOutlinedIcon from '@material-ui/icons/LockOutlined';
import Typography from '@material-ui/core/Typography';
import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import Alert from '@material-ui/lab/Alert';

import { userSignUp } from '../Api';

const Copyright = () => {
    return (
        <Typography variant="body2" color="textSecondary" align="center">
            {'Copyright © MindboxDevSchool Mindhub Project'}
            {' '}
            {new Date().getFullYear()}
            {'.'}
        </Typography>
    );
}

const useStyles = makeStyles((theme) => ({
    paper: {
        marginTop: theme.spacing(8),
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
    },
    avatar: {
        margin: theme.spacing(1),
        backgroundColor: theme.palette.secondary.main,
    },
    form: {
        width: '100%', // Fix IE 11 issue.
        marginTop: theme.spacing(3),
    },
    submit: {
        margin: theme.spacing(3, 0, 2),
    },
}));


const UserSignUp = () => {
    const history = useHistory();

    const signIn = async (username, password, repeatPassword) => {
        if (password === repeatPassword) {
            const {accessToken, errors} = await userSignUp(username, password);
            if (accessToken) {
                localStorage.setItem("accessToken", accessToken);
                localStorage.setItem("username", username);
                history.push("/")
            }
            if (errors) {
                setCommonError(errors["commonError"]);
                const {usernameError} = errors;
                setUsernameHelperText(usernameError);
            }
        } else {
            setPasswordHelperText("Passwords do not match.")
        }
    }

    const classes = useStyles();

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");

    const [usernameHelperText, setUsernameHelperText] = useState("")
    const [passwordHelperText, setPasswordHelperText] = useState("")

    const [commonError, setCommonError] = useState("");
    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline />
            <div className={classes.paper}>
            <Avatar className={classes.avatar}>
                <LockOutlinedIcon/>
            </Avatar>
            <Typography component="h1" variant="h5">
                Sign up
            </Typography>
            <form
                className={classes.form}
                onSubmit={(e) => {
                    e.preventDefault();
                    signIn(username, password, repeatPassword);
                }}
            >
                <Grid container spacing={2}>
                    {commonError ? 
                    <Grid item xs={12}>
                        <Alert variant="outlined" severity="error">
                            {commonError}
                        </Alert>
                    </Grid>
                    : []}
                    <Grid item xs={12}>
                        <TextField
                        autoComplete="login"
                        name="username"
                        variant="outlined"
                        required
                        fullWidth
                        id="username"
                        label="Username"
                        autoFocus
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        error={usernameHelperText ? true : false}
                        helperText = {usernameHelperText}
                        />
                    </Grid> 
                    <Grid item xs={12}>
                        <TextField
                        variant="outlined"
                        required
                        fullWidth
                        name="password"
                        label="Password"
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        error={passwordHelperText ? true : false}
                        helperText = {passwordHelperText}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField
                        variant="outlined"
                        required
                        fullWidth
                        name="repeat-password"
                        label="Repeat Password"
                        type="password"
                        id="repeat-password"
                        value={repeatPassword}
                        onChange={(e) => setRepeatPassword(e.target.value)}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <FormControlLabel
                        control={<Checkbox value="funCheckbox" color="primary" required/>}
                        label="I want to sell myself into slavery."
                        />
                    </Grid>
                </Grid>
                <Button
                type="submit"
                fullWidth
                variant="contained"
                color="primary"
                className={classes.submit}
                >
                    Sign Up
                </Button>
                <Grid container justify="flex-end">
                    <Grid item>
                        <Link
                        variant="body2"
                        to="/users/signin"
                        >
                        Already have an account? Sign in
                        </Link>
                    </Grid>
                </Grid>
            </form>
            </div>
            <Box mt={5}>
            <Copyright />
            </Box>
        </Container>
    );
}

export default UserSignUp;