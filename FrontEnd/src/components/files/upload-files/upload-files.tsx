import * as React from 'react';
import PositionedSnackbar from "../../../helpers/notify-msg";
import ObjectStorage from "../../../helpers/ObjectStorage";
import Card from '@material-ui/core/Card';
import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import Typography from '@material-ui/core/Typography';
import { Input } from '@material-ui/core';
import Grid from '@material-ui/core/Grid';
import Button from "@material-ui/core/Button";
import { urlAPI } from "../../../helpers/constants";
import axios from 'axios';

type MyProps = {
};

type MyState = {
    files: any;
    open: boolean;
    can_close: boolean;
    errorMessage: string;
};

export default class UploadFiles extends React.Component<MyProps, MyState> {

    objectStorage: any;

    state: MyState = {
        files: null,
        open: false,
        can_close: true,
        errorMessage: "",
    };

    constructor(props: MyProps) {
        super(props);
        this.objectStorage = new ObjectStorage();
    }
    
    notify = () => {
        this.setState({
            open: false || !this.state.can_close,
        });
        this.setCanClose(true);
    };

    setCanClose = (lock: boolean) => {
        this.setState({
            can_close: lock,
        });
    }

    setErrorMessage(message: string) {
        this.setState({
            open: true,
            errorMessage: message
        });
    }

    uploadFiles = (e: any) => {
        const new_files = e.target.files;
    
        this.setState({
          files: new_files,
        });

        this.setErrorMessage("Files in cache");
    }

    uploadFilesBlob = () => {
        const files = this.state.files;
    
        this.setCanClose(false);
        this.setErrorMessage("Loading...");
        this.objectStorage.uploadFiles(files, (response: any) => {
            this.setCanClose(true);
        
            const files_to_send: any = [];
            response.forEach(function (value: any, index: number) {
                let url = value._response.request.url;
                url = url.slice(0, url.indexOf("?"));
                const title = files[index].name;

                files_to_send.push({url: url, title: title});
            }); 

            this.setErrorMessage("Files in storage");
            
            //console.log("BODY", files_to_send);
            axios.post(urlAPI + 'documents/notify', files_to_send).then((response) => {
                //console.log(response);
                this.setErrorMessage("Files uploaded");
            });
        });
      }

      render() {
        return (
            <>

      <Grid container spacing={2}>
        <Grid item xs={12}>
            <h1 style={{textAlign: 'center'}}>Welcome to DocumentAnalyzer!</h1>
        </Grid>

        <Grid item xs={12}>
            <Card>
                <br/>
                <Grid item xs={12}>
                    <CardContent>
                        <Typography variant="h4" component="h2" style={{textAlign: 'center'}}>
                            Upload files
                        </Typography>
                        <Typography color="textSecondary" style={{textAlign: 'center'}}>
                            This action will start the analysis
                        </Typography>
                    </CardContent>
                </Grid>
                <CardActions>
                    <Grid item xs={12}>
                        <h5 style={{textAlign: 'center'}}><Input inputProps={{ multiple: true }} type="file" onChange={this.uploadFiles} style={{alignContent: 'center'}}/></h5>
                    </Grid>

                    <Grid item xs={12}>
                        <h5 style={{textAlign: 'center'}}>
                            <Button style={{ backgroundColor: "#5D5C61", color: "white", textTransform: 'capitalize', alignContent: "center" }}  variant="contained" onClick={this.uploadFilesBlob}>Upload files</Button>
                        </h5>
                    </Grid>
                </CardActions>
                <PositionedSnackbar message={this.state.errorMessage} open_msg={this.state.open} close={this.notify} />
                <br/>
            </Card>
        </Grid>

      </Grid>
            
            </>
        );
      }

}