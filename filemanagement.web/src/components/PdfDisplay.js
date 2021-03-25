import React, { useState } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import { Button } from "semantic-ui-react";
import { useTranslation } from "react-i18next";

import "../styles/fileViewer.css";
pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

const PdfDisplay = (props) => {
  const [numPages, setNumPages] = useState(null);
  const [pageNumber, setPageNumber] = useState(1);
  const { t } = useTranslation();

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
          onClick={() => {
            setPageNumber(pageNumber - 1);
          }}
          disabled={pageNumber <= 1 ? true : false}
        >
          {t("pdfDisplay.goBack")}
        </Button>
        <p>
          {pageNumber} / {numPages}
        </p>
        <Button
          onClick={() => {
            setPageNumber(pageNumber + 1);
          }}
          disabled={pageNumber === numPages ? true : false}
        >
          {t("pdfDisplay.goForward")}
        </Button>
      </div>
    </div>
  );
};

export default PdfDisplay;
