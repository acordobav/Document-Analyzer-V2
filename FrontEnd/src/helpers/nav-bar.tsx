import React from 'react';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';
import useStyles from "./style/general";
import Grid from '@material-ui/core/Grid';
import { Link } from 'react-router-dom';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';
import { AuthService } from '../auth/AuthService';

interface TabPanelProps {
  children?: any;
  index: any;
  value: any;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box p={3}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

function a11yProps(index: any) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

const NavBar = (props: any) => {
  const classes = useStyles();
  const [value, setValue] = React.useState(0);

  const handleChange = (event: React.ChangeEvent<{}>, newValue: number) => {
    setValue(newValue);
  };

  const auth = AuthService.getInstance();
  const onLogout = () => {
    auth.logout();
  }

  // console.log(auth.getToken());

  return (
    <div className={classes.root}>
      <AppBar position="static">
        <Grid container direction="row" justify="space-between" alignItems="center">
          <Grid item>
            <Tabs value={value} onChange={handleChange} aria-label="simple tabs example">
              <Tab label="Upload file" {...a11yProps(0)} />
              <Tab label="Files results" {...a11yProps(1)} />
              <Tab label="Employees results" {...a11yProps(2)} />
            </Tabs>  
          </Grid>
          <Grid item style={{marginRight: 50}}>
            <Link onClick={onLogout} className={classes.link} to='/'><ExitToAppIcon></ExitToAppIcon></Link>
          </Grid>
        </Grid>      
      </AppBar>

      <TabPanel value={value} index={0}>
        {props.nav0}
      </TabPanel>
      <TabPanel value={value} index={1}>
        {props.nav1}
      </TabPanel>
      <TabPanel value={value} index={2}>
        {props.nav2}
      </TabPanel>
    </div>
  );
}

export default NavBar;
