import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { getStats, getStatsForTracker } from '../Api'

import { makeStyles } from '@material-ui/core/styles';
import Container from '@material-ui/core/Container';
import StatsFactCard from '../Components/Stats/StatsFactCard';

const useStyles = makeStyles((theme) => ({
    statsFacts: {}
}));

const StatsView = () => {
    const classes = useStyles();
    const { trackerId } = useParams();
    const [stats, setStats] = useState([]);

    const fetchStats = async (trackerId = null) => {
        const facts = trackerId == null ? await getStats() : await getStatsForTracker(trackerId)
        facts.sort((a, b) => b.priority - a.priority); // Sort by priority, descending
        setStats(facts);
    };

    useEffect(() => fetchStats(trackerId), []);

    return (
        <Container component="main" maxWidth="xs">
            <h3>А вы знали?</h3>
            <div className={classes.statsFacts}>
                {stats.map(fact => <StatsFactCard {...fact}/>)}
            </div>
        </Container>
    );
};

export default StatsView;
