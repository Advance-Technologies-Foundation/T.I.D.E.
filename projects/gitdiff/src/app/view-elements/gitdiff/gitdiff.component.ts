import {Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation} from "@angular/core";
import * as Diff2Html from 'diff2html';
import { CrtInterfaceDesignerItem,  CrtViewElement} from "@creatio-devkit/common";

@CrtViewElement({
  selector: "atf-gitdiff",
  type: "atf.GitDiff",
})
@CrtInterfaceDesignerItem({
  toolbarConfig: {
    caption: "Your component",
    name: "atf-gitdiff",
    icon: require("!!raw-loader?{esModule:false}!./gitdiff-icon.svg"),
  },
})
@Component({
  selector: "atf-gitdiff",
  templateUrl: './gitdiff.component.html',
  encapsulation: ViewEncapsulation.ShadowDom,
  styleUrls: ['./gitdiff.component.css']
})
export class GitDiffComponent implements OnInit{
  @ViewChild('diffContainer', { static: true }) diffContainer!: ElementRef;
  constructor() { }

  @Input() content= '';

  ngOnInit(): void {   
    this.showDiff();
  }

  showDiff(): void {
    const diffString = `--- a/file1.txt
+++ b/file2.txt
@@ -1,3 +1,3 @@
-Hello World
+Hello AtfTIDE
 This is a diff example. That will show diff in Creatio`;

    const diffHtml = Diff2Html.html(diffString, {
      drawFileList: true,
      matching: 'lines',
      outputFormat: 'side-by-side'
    });

      this.content = diffHtml;
  }

}