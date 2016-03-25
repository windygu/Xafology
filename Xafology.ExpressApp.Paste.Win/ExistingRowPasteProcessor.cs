﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Win
{
    public class ExistingRowPasteProcessor
    {
        private readonly ICopyParser copyParser;
        private readonly View view;

        public ExistingRowPasteProcessor(ICopyParser copyParser, View view)
        {
            this.copyParser = copyParser;
            this.view = view;
        }

        // note that the new row must be focused for this to work
        public void Process(int[] selectedRowHandles)
        {
            GridListEditor listEditor = ((ListView)view).Editor as GridListEditor;
            var gridView = listEditor.GridView;
            var copiedValues = copyParser.ToArray();

            // paste rows
            for (int r = 0; r < copiedValues.Length; r++)
            {
                // ignore row with empty string
                if (copiedValues[r].Length == 1 && string.IsNullOrWhiteSpace(copiedValues[r][0]))
                    continue;

                // add new row in gridview
                gridView.FocusedRowHandle = selectedRowHandles[r];

                // paste cells
                PasteUtils.PasteColumnsToRow(copiedValues[r], gridView.FocusedRowHandle,
                    listEditor, view.ObjectSpace);

                gridView.UpdateCurrentRow();
            }
        }
    }
}
