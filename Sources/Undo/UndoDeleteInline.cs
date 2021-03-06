﻿/*
    Copyright (c) 2005-2012 Fedir Nepyivoda <fednep@gmail.com>
  
    This file is part of UV Outliner project.
    http://uvoutliner.com

    UV Outliner is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    UV Outliner is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with UV Outliner.  If not, see <http://www.gnu.org/licenses/>
 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace UVOutliner.Undo
{
    class UndoDeleteInline : DocumentUndoAction
    {
        int __NoteId;
        MemoryStream __Document;
        object __DocumentTag;

        public UndoDeleteInline(OutlinerNote note)
        {
            __NoteId = note.Id;
            __Document = DocumentHelpers.SaveDocumentToStream(note.InlineNoteDocument);
            __DocumentTag = note.InlineNoteDocument.Tag;
        }

        public override void Undo(OutlinerDocument document, TreeListView treeListView)
        {
            var note = document.FindOutlinerNoteById(__NoteId);
            if (note == null)
                return;

            treeListView.MakeActive(note, -1, false);
            note.CreateInlineNote(DocumentHelpers.RestoreDocumentFromStream(__Document));
            note.InlineNoteDocument.Tag = __DocumentTag;
        }

        public override void Redo(OutlinerDocument document, TreeListView treeListView)
        {
            var note = document.FindOutlinerNoteById(__NoteId);
            if (note == null)
                return;

            note.Document.FocusEditAfterTemplateChange = true;
            note.RemoveInlineNoteIfEmpty();
        }
    }
}
