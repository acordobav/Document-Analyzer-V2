import React, { useState, useEffect } from 'react';
import Button from "@material-ui/core/Button";
import { GridColDef, GridValueGetterParams } from "@material-ui/data-grid";
import DataTable from "../../../helpers/table";
import GenericModal from "../../../helpers/generic-modal";
import { Typography } from '@material-ui/core';
import IconButton from '@material-ui/core/IconButton';
import SyncIcon from '@material-ui/icons/Sync';
// import employee from "../../../data/employee";
import axios from 'axios';
import { urlAPI } from "../../../helpers/constants";

const columns: GridColDef[] = [
  // { field: 'id', headerName: 'ID', width: 70 },
  { 
    field: 'employeeName', 
    headerName: 'Name', 
    width: 500,
    headerAlign: 'left',
    align: "left",
  },
  { 
    field: 'count', 
    headerName: 'Total Occurrences', 
    width: 300,
    headerAlign: 'left',
    align: "left",
    type: 'number',
  },
  {
    field: 'documents',
    headerName: 'Ocurrences in documents',
    description: 'This column is not sortable.',
    sortable: false,
    width: 300,
    headerAlign: 'left',
    align: "left",

    renderCell: (params: GridValueGetterParams) => {
        const documents: any = params.getValue("documents")!;

        const items: JSX.Element[] = [];
        for(const employee of documents){
          items.push(<div key={employee.name}><Typography>{employee.name}: {employee.qty}</Typography></div>);
        }

        if(documents.length === 0){
            return <Button style={{ backgroundColor: "#5D5C61", color: "white", textTransform: 'capitalize' }} variant="contained" disabled>No occurrences</Button>
        } else {
            return <GenericModal>
                        <h2>References in documents</h2>
                        <hr/>
                        {items}
                    </GenericModal>;
        }
    }
  }
];


export default (() => {

  const [data, setData] = useState([{id: 0, employeeName: "No employees", count: 0, documents: [{}] }]);

  function setNewData(){
    axios.get(urlAPI + 'documents/users/count').then(
        response => {
          response.data.forEach( (element: any, index: number) => {
            element["id"] = index;
          });
          //console.log(response.data);
          setData(response.data);
        }
    );
  }

  useEffect(() => {
    // setData(employee); // Descomentar para usar datos hardcode
    setNewData(); 
  }, [])

  function onClick() {
    // const new_rows: any = [];
    // data.forEach(val => new_rows.push(Object.assign({}, val)));
    // new_rows[0].employeeName = "Obi Wan Kenobi";
    // new_rows.push({id: 0, employeeName: "Anakin Skywalker", count: 8, documents: [{name: "Episodio III", qty: 430}, {name: "Episodio VI", qty: 1138}] });
    // setData(new_rows);
    setNewData();
  }

  return (
      <>
        <h2>
          Results employees
          <IconButton onClick={onClick}>
            <SyncIcon />
          </IconButton>
        </h2>
        <DataTable  rows={data} columns={columns} pageSize={5} height={400}/>
      </>
  );
}) as React.SFC;
