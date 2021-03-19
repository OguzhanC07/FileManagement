import React, { useState } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import { Button } from "semantic-ui-react";

import "../styles/fileViewer.css";
pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

const PdfDisplay = (props) => {
  const [numPages, setNumPages] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);

  const onDocumentLoadSuccess = ({ numPages }) => {
    setNumPages(numPages);
  };

  return (
    <div className="outer">
      <Document
        className="inner"
        file={props.file}
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
  );
};

export default PdfDisplay;
