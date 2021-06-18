import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const useStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      flexGrow: 1,
    },

    link: {
      color: "#FFFFFF"
    },
    
  }),
);
export default useStyles;


