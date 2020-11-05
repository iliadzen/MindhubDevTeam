import React, { useEffect, useState } from 'react';
import {Link, useHistory} from 'react-router-dom';
import { makeStyles } from '@material-ui/core/styles';
import List from '@material-ui/core/List';
import ListItem from '@material-ui/core/ListItem';
import ListItemText from '@material-ui/core/ListItemText';
import ListItemAvatar from '@material-ui/core/ListItemAvatar';
import Avatar from '@material-ui/core/Avatar';
import Divider from '@material-ui/core/Divider';
import Button from '@material-ui/core/Button';
import CssBaseline from '@material-ui/core/CssBaseline';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import Container from '@material-ui/core/Container';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import AccountCircleOutlinedIcon from '@material-ui/icons/AccountCircleOutlined';
import RecentActorsOutlinedIcon from '@material-ui/icons/RecentActorsOutlined';
import CalendarTodayOutlinedIcon from '@material-ui/icons/CalendarTodayOutlined';
import DateRangeOutlinedIcon from '@material-ui/icons/DateRangeOutlined';
import { getSelf } from '../Api';

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
        margin: theme.spacing(0.5, 0, 0),
    },
}));

export default function UserApp() {
    const history = useHistory();
    const classes = useStyles();
    const [userContent, setUserContent] = useState({username:"Loading...", creationDate:"Loading...", modificationDate:"Loading..."});
    const fetchUserContent = async () => {
            const {username, creationDate, modificationDate} = await getSelf();
            const newContent = {username, creationDate, modificationDate};
            setUserContent(newContent);
    }
    useEffect(() => {
        fetchUserContent();
    }, []);
    return (
    <Container component="main" maxWidth="xs">
        <Button
                style={{marginTop: "10px",}}
                variant="contained"
                color="primary"
                className={classes.submit}
                onClick={() => {
                    history.push(`/`);
                }}
            >
                Go Back
        </Button>
        <CssBaseline />
        <div className={classes.paper}>
            <Avatar className={classes.avatar}>
                <AccountCircleOutlinedIcon />
            </Avatar>
            <Typography component="h1" variant="h5">
                Profile
            </Typography>
            <Grid container spacing={2}>
                <Grid item xs={12}>
                    <Card variant="outlined">
                        <CardContent>
                            <List>
                                <ListItem>
                                    <ListItemAvatar>
                                    <Avatar>
                                        <RecentActorsOutlinedIcon />
                                    </Avatar>
                                    </ListItemAvatar>
                                    <ListItemText primary="Username" secondary={userContent.username} />
                                </ListItem>
                                <Divider variant="inset" component="li" />
                                <ListItem>
                                    <ListItemAvatar>
                                    <Avatar>
                                        <CalendarTodayOutlinedIcon />
                                    </Avatar>
                                    </ListItemAvatar>
                                    <ListItemText primary="Creation Date" secondary={userContent.creationDate} />
                                </ListItem>
                                <Divider variant="inset" component="li" />
                                <ListItem>
                                    <ListItemAvatar>
                                    <Avatar>
                                        <DateRangeOutlinedIcon />
                                    </Avatar>
                                    </ListItemAvatar>
                                    <ListItemText primary="Modification Date" secondary={userContent.modificationDate} />
                                </ListItem>
                            </List>
                        </CardContent>
                    </Card>
                </Grid>
            </Grid>
            <Button
                fullWidth
                variant="contained"
                color="secondary"
                className={classes.submit}
                onClick={() => {
                    history.push("/users/edit");
                }}
            >
                Edit Profile
            </Button>
        </div>
        </Container>
  );
}