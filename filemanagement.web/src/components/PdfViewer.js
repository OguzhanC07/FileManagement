import React, { useState } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import { Modal, Icon, Button } from "semantic-ui-react";

import { getsinglefile } from "../services/fileService";
import "../styles/fileViewer.css";
pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

const PdfViewer = (props) => {
  const [numPages, setNumPages] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const [open, setOpen] = useState(false);
  const [file, setFile] = useState("");

  const onDocumentLoadSuccess = ({ numPages }) => {
    setNumPages(numPages);
  };

  const pdfViewHandler = async (e) => {
    e.preventDefault();
    const response = await getsinglefile(props.id);
    const base64 = await convertBase64(response.data);
    setFile(base64);
    setOpen(true);
  };

  const convertBase64 = (blob) => {
    return new Promise((resolve, reject) => {
      const fileReader = new FileReader();
      fileReader.readAsDataURL(blob);

      fileReader.onload = () => {
        resolve(fileReader.result);
      };

      fileReader.onerror = (error) => {
        reject(error);
      };
    });
  };

  return (
    <span>
      <Icon
        name="external square alternate"
        onClick={(e) => {
          pdfViewHandler(e);
        }}
      />
      <Modal
        onOpen={() => setOpen(true)}
        basic
        closeIcon
        open={open}
        onClose={() => {
          setOpen(false);
        }}
      >
        <div className="outer">
          <Document
            className="inner"
            file={file}
            onLoadSuccess={onDocumentLoadSuccess}
          >
            <Page pageNumber={pageNumber} />
          </Document>
          <div className="outer">
            <Button
              className="inner"
              onClick={() => {
                setPageNumber(pageNumber - 1);
              }}
              disabled={pageNumber <= 1 ? true : false}
            >
              Go Back
            </Button>
            <p>
              Page {pageNumber} of {numPages}
            </p>
            <Button
              className="inner"
              onClick={() => {
                setPageNumber(pageNumber + 1);
              }}
              disabled={pageNumber === numPages ? true : false}
            >
              Go Forward
            </Button>
          </div>
        </div>
      </Modal>
    </span>
  );
};

export default PdfViewer;
