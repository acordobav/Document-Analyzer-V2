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

    name: string;
    email: string;
    password1: string;
    password2: string;
};

export default  class SignUp extends React.Component<MyProps, MyState> {

  state: MyState = {
    valid_data: false,
    open: false,
    errorMessage: "",
    name: "",
    email: "",
    password1: "",
    password2: "",
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

  setValidData(state: boolean) {
    this.setState({
      valid_data: state
    });
  }

    handleChange = (e: { target: { name: any; value: any; type: any; checked?: any }; }) => {
        const {name, value, type, checked} = e.target;

        if(type === "checkbox"){
            const e_password1 = (document.querySelector('#password1') as HTMLInputElement);
            const e_password2 = (document.querySelector('#password2') as HTMLInputElement);
            if(checked) {
              e_password1.type = "text";
              e_password2.type = "text";
            } else {
              e_password1.type = "password";
              e_password2.type = "password";
            }
        } else {
            const new_state = { [name]: value } as Pick<MyState, keyof MyState>;
            this.setState(new_state);
        }
    }

  validateInputs() {
    this.setState({ errorMessage: "" });

    const name = this.state.name;
    const email = this.state.email;
    const password1 = this.state.password1;
    const password2 = this.state.password2;

    if (name === "" || email === "" || password1 === "" || password2 === "") {
      this.setState({
        errorMessage: "Empty fields",
      });
      return false;
    }

    if (password1 !== password2) {
      this.setState({
        errorMessage: "Passwords don't match",
      });
      return false;
    }

    const re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if(!re.test(email)) {
      this.setState({
        errorMessage: "Invalid format: email",
      });
      return false;
    }

    return true;
  }

  onSubmit = (e: any) => {
    e.preventDefault();

    // Validation of the input fields
    const is_valid = this.validateInputs();
    if(!is_valid){
      this.notify();
    } else {
      this.setValidData(true);
    }

    const name = this.state.name;
    const email = this.state.email;
    const password1 = this.state.password1;

    console.log(name, email, password1);

    // SignupService.signupRequest(name, email, password1).then(
    //   () => {
    //     this.props.history.push("/login");
    //   }
    // ).catch(
    //   (error) => {
    //     let errorInfo = "Error desconocido.";
        
    //     try {
    //       if(!!error.response) {
    //         const errorData = error.response.data;
    //         switch (errorData) {
    //           case "EMAILS_EXISTS":
    //             errorInfo = "Correo ya está registrado."
    //             break;
            
    //           default:
    //             break;
    //         }
    //       }
    //     }
    //     catch {}

    //     this.setState({
    //       errorMessage: errorInfo,
    //     });
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
                    id="name"
                    label="Full name"
                    name="name"
                    autoComplete="name"
                    autoFocus
                    onChange={this.handleChange}
                  />
                  <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="email"
                    label="Email Address"
                    name="email"
                    autoComplete="email"
                    onChange={this.handleChange}
                  />
                  <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    name="password1"
                    label="Password"
                    type="password"
                    id="password1"
                    autoComplete="current-password"
                    onChange={this.handleChange}
                  />
                  <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    name="password2"
                    label="Confirm password"
                    type="password"
                    id="password2"
                    autoComplete="current-password"
                    onChange={this.handleChange}
                  />
                  <FormControlLabel
                    control={<Checkbox color="primary" onChange={this.handleChange}/>}
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
                    Sign Up
                  </Button>
                  <br/>
                  <hr/>
                  <Grid container>
                    <Grid item>
                      <Link href='/sign_in'>Do you already have an account? Sign In</Link>
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