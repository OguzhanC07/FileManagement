import React, { useState } from "react";
import { Document, Page, pdfjs } from "react-pdf";
import { Modal, Icon } from "semantic-ui-react";

// import pdf from "../assets/file.pdf";
pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

const PdfViewer = (props) => {
  const [numPages, setNumPages] = useState(null);
  const [open, setOpen] = useState(false);

  const [pageNumber, setPageNumber] = useState(1);

  const onDocumentLoadSuccess = ({ numPages }) => {
    setNumPages(numPages);
  };

  return (
    <div>
      <Icon
        name="external square alternate"
        onClick={() => {
          setOpen(true);
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
        {/* <Document file={pdf} onLoadSuccess={onDocumentLoadSuccess}>
          <Page pageNumber={pageNumber} />
        </Document> */}
        <p>
          Page {pageNumber} of {numPages}
        </p>
      </Modal>
    </div>
  );
};

export default PdfViewer;
