import { makeStyles } from '@material-ui/core/styles';
import Card from '@material-ui/core/Card';
import CardContent from '@material-ui/core/CardContent';
import CardActions from '@material-ui/core/CardActions';
import LinkButton from '../Common/LinkButton';

const useStyles = makeStyles((theme) => ({
    card: {
        margin: "50px 20px 5px 20px",
    }
}));

const StatsFactCard = ({trackerId, description}) => {
    const classes = useStyles();
    return (
        <Card className={classes.card}>
            <CardContent>
                <p>{description}</p>
            </CardContent>
            <CardActions>
                {trackerId != null ? <LinkButton url={`/trackers/${trackerId}`} text="Трекер"/> : []}
            </CardActions>
        </Card>
    );
}

export default StatsFactCard;
