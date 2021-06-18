import React, { useState, useEffect, useMemo } from 'react';
import Button from "@material-ui/core/Button";
import { GridColDef, GridValueGetterParams } from "@material-ui/data-grid";
import DataTable from "../../../helpers/table";
import GenericModal from "../../../helpers/generic-modal";
import { Typography } from '@material-ui/core';
import ChildFriendlyIcon from '@material-ui/icons/ChildFriendly';
import Tooltip from '@material-ui/core/Tooltip';
//import { w3cwebsocket as W3CWebSocket } from "websocket";
// import files from "../../../data/files";
import InsertEmoticonIcon from '@material-ui/icons/InsertEmoticon';
import SentimentSatisfiedIcon from '@material-ui/icons/SentimentSatisfied';
import SentimentVeryDissatisfiedIcon from '@material-ui/icons/SentimentVeryDissatisfied';
import axios from "axios";
import { urlAPI, WEB_SOCKET } from "../../../helpers/constants";

const columns: GridColDef[] = [
  // { field: 'id', headerName: 'ID', width: 70 },
  { 
    field: 'title', 
    headerName: 'Filename', 
    width: 500,
    headerAlign: 'left',
    align: "left",
    renderCell: (params: GridValueGetterParams) => {
      const title: string = `${params.getValue("title")}`;
      const url: string = `${params.getValue("url")}`;

      return <a href={url}>{title}</a>
    }
  },

  {
    field: 'status',
    headerName: 'Status',
    description: 'This column is not sortable.',
    sortable: false,
    width: 400,
    headerAlign: 'left',
    align: "left",
    //type: 'number',

    renderCell: (params: GridValueGetterParams) => {
        const status: boolean = !!params.getValue("status")!;
        const documents: any = params.getValue("userDocumentReferences")!;

        const items: JSX.Element[] = [];
        for(const file of documents){
          items.push(<div key={file.name}><Typography>{file.name}: {file.qty}</Typography></div>);
        }

        if(status){
          if(documents.length === 0){
            return <Button style={{ backgroundColor: "#379683", color: "white", textTransform: 'capitalize' }} variant="contained" disabled>No matches</Button>
          }
          return <GenericModal>
                  <h2>Employees in documents</h2>
                  <hr/>
                  {items}
                </GenericModal>;
        }
        return <Button style={{ backgroundColor: "#5D5C61", color: "white", textTransform: 'capitalize' }} variant="contained" disabled>Not processed</Button>
    }
  },

  {
    field: 'feelings',
    headerName: 'Feelings',
    description: 'This column is not sortable.',
    sortable: false,
    width: 400,
    headerAlign: 'left',
    align: "left",

    renderCell: (params: GridValueGetterParams) => {
        const status: boolean = !!params.getValue("status")!;
        const feelings: any = params.getValue("feelings")!;

        const items: JSX.Element[] = [];
        feelings.forEach(function (value: any) {    
          
          if(value.name === "Positive"){
            items.push(<Tooltip title={`${value.name}: ${value.score}%`}><InsertEmoticonIcon/></Tooltip>);
          } else if(value.name === "Neutral"){
            items.push(<Tooltip title={`${value.name}: ${value.score}%`}><SentimentSatisfiedIcon/></Tooltip>);
          } else if(value.name === "Negative"){
            items.push(<Tooltip title={`${value.name}: ${value.score}%`}><SentimentVeryDissatisfiedIcon/></Tooltip>);
          }

        }); 

        if(status){
          if(feelings.length === 0){
            return <Typography>No feelings detected</Typography>
          }
          return <Typography>{items}</Typography>;
        }
        return <Typography>Not processed</Typography>
    }
  },

  {
    field: 'offensiveContent',
    headerName: 'Obscene language',
    description: 'This column is not sortable.',
    sortable: false,
    width: 400,
    headerAlign: 'left',
    align: "left",

    // renderCell: (params: GridValueGetterParams) => {
    //     const status: boolean = !!params.getValue("status")!;
    //     const is_obscene: boolean = !!params.getValue("offensiveContent")!;

    //     if(status){
    //       if(is_obscene){
    //         return <Tooltip title="This document contains obscene language"><ExplicitIcon /></Tooltip>
    //       } else {
    //         return <Tooltip title="This document is child friendly"><ChildFriendlyIcon /></Tooltip>
    //       }
    //     }

    //     return <Typography>Not processed</Typography>
    // }

    renderCell: (params: GridValueGetterParams) => {
      const status: boolean = !!params.getValue("status")!;
      const dirty_words: any = params.getValue("obscene_language")!;
      // console.log("DirtyWords: ", dirty_words)
      

      var dirty_words_str: string = "";
      for(const word of dirty_words){
        dirty_words_str = dirty_words_str + `${word}, `;
      }

      dirty_words_str = dirty_words_str.slice(0, -2); 

      if(status){
        if(dirty_words.length === 0){
          return <Tooltip title="This document is child friendly"><ChildFriendlyIcon /></Tooltip>
        }
        return <GenericModal>
                <h2>Offensive words detected</h2>
                <hr/>
                <p>{dirty_words_str}</p>
              </GenericModal>;
      }
      return <Typography>Not processed</Typography>
  }


  },

];



export default (() => {
  
  const [data, setData] = useState([{id: "0", title: "No files", status: false, feelings: [{}], obscene_language: [""], url: "google.com", userDocumentReferences: [{}] }]);

  const client = useMemo(() => new WebSocket(WEB_SOCKET), []);

  useEffect(() => {
    //setData(files); // Descomentar para usar datos hardcode
    axios.get(urlAPI + "documents").then((response) => {
      // console.log("Http ", response.data)
      setData(response.data);
    });

    client.onopen = () => {
      //console.log('WebSocket Client Connected');
    };

    client.onclose = () => {
      //console.log('WebSocket Client Disconnected');
    };

    return () => {
      client.close();
    }

  }, [client]);
  
  useEffect(() => {
    client.onmessage = (message) => {
      let response = message.data.toString();
      const received_data = JSON.parse(response);
      /* response = response.replace("Name","name");
      response = response.replace("Score","score");
      
      const feelings_wrong = received_data.feelings;
      for (let feel of feelings_wrong) {
        feel = renameKey(feel, "Name", "name");
        feel = renameKey(feel, "Score", "score");

      }*/

      // console.log("WebSocket: ", received_data);

      const new_rows: any = [];
      data.forEach(val => new_rows.push(Object.assign({}, val)));
      
      let new_item: boolean = true;
      for( var i = 0; i < new_rows.length; i++){   
        if(new_rows[i].id === received_data.id) { 
          new_rows[i] = received_data;
          new_item = false;
        }
      }

      if(new_item){
        new_rows.push(received_data);
      }

      setData(new_rows);
    };

  }, [data, client]);

    // client.send(JSON.stringify({
    //   data: "Bye",
    //   type: "unsubscribe"
    // }));
  
  return (
      <>
        <h2>Results files</h2>
        <DataTable  rows={data} columns={columns} pageSize={5} height={400}/>
      </>
  );
}) as React.SFC;
/*
function renameKey(object: any, key: any, newKey: any) {

  const clonedObj = {...object};

  const targetKey = clonedObj[key];



  delete clonedObj[key];

  clonedObj[newKey] = targetKey;

  return clonedObj;

};*/