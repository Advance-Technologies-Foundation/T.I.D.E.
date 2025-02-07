import {Component, ElementRef, Input, OnInit, ViewChild, ViewEncapsulation} from "@angular/core";
import * as Diff2Html from 'diff2html';
import {CrtInput, CrtInterfaceDesignerItem, CrtViewElement} from "@creatio-devkit/common";

@CrtViewElement({
  selector: "atf-gitdiff",
  type: "atf.GitDiff",
  inputs: {
    diffContent: ""
  },
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
  public htmlContentInternal= '';
  private _diffContent : string = '';
  public get diffContent() : string {
    return this._diffContent;
  }
  @Input()
  @CrtInput()
  public set diffContent(v : string) {
    
    if(v && v.length > 0 && v !== this._diffContent){
      this._diffContent = v;
      this.showDiff();
    }
  }
  
  ngOnInit(): void {   
    //this.showDiff();
  }

  showDiff(): void {
    
    if(this.diffContent && this.diffContent.length > 0){
      this.htmlContentInternal = Diff2Html.html(this.diffContent, {
          drawFileList: true,
          matching: 'lines',
          outputFormat: 'side-by-side'
        });
    }
  }
}