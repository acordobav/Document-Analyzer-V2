import * as React from 'react';
import Link from '@material-ui/core/Link';
import PositionedSnackbar from "../../../helpers/notify-msg";
import Button from '@material-ui/core/Button';
import  { Redirect } from 'react-router-dom' 
import FormControlLabel from '@material-ui/core/FormControlLabel';
import Checkbox from '@material-ui/core/Checkbox';
import Box from '@material-ui/core/Box';
import Grid from '@material-ui/core/Grid';
import TextField from '@material-ui/core/TextField';
import Copyright from "../../../helpers/copyright";

type MyProps = {
    message: string;
};

type MyState = {
    valid_data: boolean; 
    open: boolean;
    errorMessage: string;
};

export default  class LogIn extends React.Component<MyProps, MyState> {

  state: MyState = {
    valid_data: false,
    open: false,
    errorMessage: "",
  };
    
  notify = () => {
    this.setState({
        open: !this.state.open,
    })
  };

  setErrorMessage(message: string) {
    this.setState({
      errorMessage: message
    });
  }

  setLogIn(state: boolean) {
    this.setState({
      valid_data: state
    });
  }

  validateInputs(email: string, password: string) {
      if(!email || !password) {
        this.setErrorMessage("Empty fields")
        return false;
      }
    
      const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      if(!re.test(email)) {
        this.setErrorMessage("Invalid format: email")
        return false;
      }

      return true;
  }

  onChange(e: React.ChangeEvent<HTMLInputElement>) {
    const target = e.target;
    if(target.type === "checkbox"){
      const e_password = (document.querySelector('#password') as HTMLInputElement);
      if(target.checked) {
        e_password.type = "text";
      } else {
        e_password.type = "password";
      }
    }
  }

  onSubmit = (e: any) => {
    e.preventDefault();

    const form = e.target.elements;

    const email = form.email.value;
    const password = form.password.value;

    // Validation of the input fields
    const is_valid = this.validateInputs(email, password);
    if(!is_valid){
      this.notify();
    } else {
      this.setLogIn(true);
    }
          
    // // Login request to the api
    // AuthService.loginRequest(email, password).then(
    //   (response) => {
    //     // The token is stored
    //     AuthService.storeToken(response.data);

    //     // Redirection to the files page
    //     this.props.history.push({
    //       pathname: '/files',
    //       state: { detail: this.state }
    //     })
    //   },
    //   () => {
    //     this.setErrorMessage("Correo electrónico o contraseña incorrectos")
    //   }
    // );
  }

    render() {
        if(this.state.valid_data){
            return <Redirect to='/files'  />
        }
        return (
            <div>

                {/* <Button onClick={this.notify}>Top-Right</Button>

                {this.props.message} {this.state.valid_data} */}

                <form noValidate onSubmit={this.onSubmit}>
                  <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="email"
                    label="Email Address"
                    name="email"
                    autoComplete="email"
                    autoFocus
                  />
                  <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    name="password"
                    label="Password"
                    type="password"
                    id="password"
                    autoComplete="current-password"
                  />
                  <FormControlLabel
                    control={<Checkbox color="primary" onChange={this.onChange}/>}
                    label="Show password"
                  />
                  <br/>
                  <br/>
                  <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    color="primary"
                  >
                    Sign In
                  </Button>
                  <br/>
                  <hr/>
                  <Grid container>
                    <Grid item>
                      <Link href='/sign_up'>Don't have an account? Sign Up</Link>
                    </Grid>
                  </Grid>
                  <Box mt={5}>
                    <Copyright />
                  </Box>
                </form>

                <PositionedSnackbar message={this.state.errorMessage} open_msg={this.state.open} close={this.notify} />
                
            </div>
        );
    }
  }