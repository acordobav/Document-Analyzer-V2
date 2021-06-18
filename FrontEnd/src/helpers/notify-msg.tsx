import React from 'react';
import Snackbar, { SnackbarOrigin } from '@material-ui/core/Snackbar';

export interface State extends SnackbarOrigin {
  open: boolean;
}

interface Snacks {
    message: string;
    open_msg: boolean;
    close: any;
}

export default function PositionedSnackbar({message, open_msg, close}: Snacks) {
  const [state] = React.useState<State>({
    open: open_msg,
    vertical: 'top',
    horizontal: 'right',
  });
  const { vertical, horizontal } = state;

  const handleClose = () => {
    close();
  };

  return (
    <div>
      <Snackbar
        anchorOrigin={{ vertical, horizontal }}
        open={open_msg}
        onClose={handleClose}
        message={message}
      />
    </div>
  );
}