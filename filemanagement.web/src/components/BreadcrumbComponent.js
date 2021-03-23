import React from "react";
import { Breadcrumb } from "semantic-ui-react";

const BreadcrumbComponent = (props) => {
  return (
    <div style={{ paddingTop: 15, paddingBottom: 10 }}>
      <Breadcrumb style={{ fontSize: 20 }}>
        <Breadcrumb.Section
          onClick={() => {
            props.removeStack(0);
          }}
        >
          /
        </Breadcrumb.Section>
        {props.folderStack.map((folder, i, { length }) =>
          length - 1 === i ? (
            <Breadcrumb.Section id={folder.id} key={folder.id}>
              {folder.folderName}
            </Breadcrumb.Section>
          ) : (
            <Breadcrumb.Section
              onClick={() => {
                props.removeStack(folder.id);
              }}
              key={folder.id}
            >
              {folder.folderName} /
            </Breadcrumb.Section>
          )
        )}
      </Breadcrumb>
    </div>
  );
};

export default BreadcrumbComponent;
