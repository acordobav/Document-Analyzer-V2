import { BlobServiceClient } from "@azure/storage-blob";
import axios from 'axios';
import { urlAPI } from "./constants";

export default class BlobStorage {

    containerClient: any;

    constructor() {
        var blobSasUrl = "";
        var blobServiceClient;

        axios.get(urlAPI + 'object').then(
            response => {
              //console.log(response.data);
              blobSasUrl = response.data;
              blobServiceClient = new BlobServiceClient(blobSasUrl);
              this.containerClient = blobServiceClient.getContainerClient("");
            }
        );
    }

    uploadFiles = async (files: any, response: any) => {

        try {
            const promises = [];
            for (const file of files) {
                const blockBlobClient = this.containerClient.getBlockBlobClient(file.name);
                promises.push(blockBlobClient.uploadBrowserData(file));
            }
            response(await Promise.all(promises));
        }
        catch (error) {
            response(error.message);
        }
    }

}
