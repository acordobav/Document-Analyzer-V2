import { makeStyles } from '@material-ui/core/styles';
import InsertEmoticonIcon from '@material-ui/icons/InsertEmoticon';
import SentimentSatisfiedIcon from '@material-ui/icons/SentimentSatisfied';
import SentimentVeryDissatisfiedIcon from '@material-ui/icons/SentimentVeryDissatisfied';

function iconStyles() {
    return {
      happyIcon: {
        color: 'green',
      },
      neutralIcon: {
        color: 'gray',
      },
      sadIcon: {
        color: 'blue',
      },
    }
  }

export default function EmojiIcon(props: { type: string }) {
    const classes = makeStyles(iconStyles)();

    if(props.type === "Positive"){
      return <InsertEmoticonIcon className={classes.happyIcon} />;
    } else if(props.type === "Neutral"){
      return <SentimentSatisfiedIcon className={classes.neutralIcon} />;
    } else if(props.type === "Negative"){
      return <SentimentVeryDissatisfiedIcon className={classes.sadIcon} />;
    }

    return <></>;
    
}