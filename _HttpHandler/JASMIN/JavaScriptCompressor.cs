//+
// Jazmin is distributed under the OSI-approved and liberal BSD License:
//
// Copyright (c) 2005, Atif Aziz. All rights reserved. Portion Copyright (c) 2001 Douglas Crockford
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//    * Neither the name of the author nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
//+
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Jazmin
{
    public sealed class JavaScriptCompressor
    {
        //
        // Public functions
        //

        public static string Compress(string source)
        {
            StringWriter writer = new StringWriter();
            Compress(new StringReader(source), writer);
            return writer.ToString();
        }

        public static void Compress(TextReader reader, TextWriter writer)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (writer == null)
                throw new ArgumentNullException("writer");

            JavaScriptCompressor compressor = new JavaScriptCompressor(reader, writer);
            compressor.Compress();
        }

        //
        // Private implementation
        //

        private int aa;
        private int bb;
        private int lookahead = eof;
        private TextReader reader = Console.In;
        private TextWriter writer = Console.Out;

        private const int eof = -1;

        private JavaScriptCompressor(TextReader reader, TextWriter writer)
        {
            Debug.Assert(reader != null);
            Debug.Assert(writer != null);

            this.reader = reader;
            this.writer = writer;
        }

        /* Compress -- Copy the input to the output, deleting the characters which are
                insignificant to JavaScript. Comments will be removed. Tabs will be
                replaced with spaces. Carriage returns will be replaced with linefeeds.
                Most spaces and linefeeds will be removed. 
        */

        private void Compress()
        {
            aa = '\n';
            Action(3);
            while (aa != eof)
            {
                switch (aa)
                {
                    case ' ':
                        if (IsAlphanum(bb))
                        {
                            Action(1);
                        }
                        else
                        {
                            Action(2);
                        }
                        break;
                    case '\n':
                    switch (bb)
                    {
                        case '{':
                        case '[':
                        case '(':
                        case '+':
                        case '-':
                            Action(1);
                            break;
                        case ' ':
                            Action(3);
                            break;
                        default:
                            if (IsAlphanum(bb))
                            {
                                Action(1);
                            }
                            else
                            {
                                Action(2);
                            }
                            break;
                    }
                        break;
                    default:
                    switch (bb)
                    {
                        case ' ':
                            if (IsAlphanum(aa))
                            {
                                Action(1);
                                break;
                            }
                            Action(3);
                            break;
                        case '\n':
                        switch (aa)
                        {
                            case '}':
                            case ']':
                            case ')':
                            case '+':
                            case '-':
                            case '"':
                            case '\'':
                                Action(1);
                                break;
                            default:
                                if (IsAlphanum(aa))
                                {
                                    Action(1);
                                }
                                else
                                {
                                    Action(3);
                                }
                                break;
                        }
                            break;
                        default:
                            Action(1);
                            break;
                    }
                        break;
                }
            }
        }

        /* Get -- return the next character from stdin. Watch out for lookahead. If 
                the character is a control character, translate it to a space or 
                linefeed.
        */

        private int Get()
        {
            int ch = lookahead;
            lookahead = eof;
            if (ch == eof)
            {
                ch = reader.Read();
            }
            if (ch >= ' ' || ch == '\n' || ch == eof)
            {
                return ch;
            }
            if (ch == '\r')
            {
                return '\n';
            }
            return ' ';
        }


        /* Peek -- get the next character without getting it.  
        */

        private int Peek()
        {
            lookahead = Get();
            return lookahead;
        }

        /* Next -- get the next character, excluding comments. Peek() is used to see 
                if a '/' is followed by a '/' or '*'.
        */

        private int Next()
        {
            int ch = Get();
            if (ch == '/')
            {
                switch (Peek())
                {
                    case '/':
                        for (;; )
                        {
                            ch = Get();
                            if (ch <= '\n')
                            {
                                return ch;
                            }
                        }
                    case '*':
                        Get();
                        for (;; )
                        {
                            switch (Get())
                            {
                                case '*':
                                    if (Peek() == '/')
                                    {
                                        Get();
                                        return ' ';
                                    }
                                    break;
                                case eof:
                                    throw new Exception("Unterminated comment.");
                            }
                        }
                    default:
                        return ch;
                }
            }
            return ch;
        }


        /* Action -- do something! What you do is determined by the argument:
                1   Output A. Copy A to B. Get the next B.
                2   Copy B to A. Get the next B. (Delete A).
                3   Get the next B. (Delete B).
           Action treats a string as a single character. Wow!
           Action recognizes a regular expression if it is preceded by ( or , or =.
        */

        private void Action(int d)
        {
            switch (d)
            {
                case 1:
                    Write(aa);
                    goto case 2;
                case 2:
                    aa = bb;
                    if (aa == '\'' || aa == '"')
                    {
                        for (;; )
                        {
                            Write(aa);
                            aa = Get();
                            if (aa == bb)
                            {
                                break;
                            }
                            if (aa <= '\n')
                            {
                                string message = string.Format("Unterminated string literal: '{0}'.", aa);
                                throw new Exception(message);
                            }
                            if (aa == '\\')
                            {
                                Write(aa);
                                aa = Get();
                            }
                        }
                    }
                    goto case 3;
                case 3:
                    bb = Next();
                    if (bb == '/' && (aa == '(' || aa == ',' || aa == '='))
                    {
                        Write(aa);
                        Write(bb);
                        for (;; )
                        {
                            aa = Get();
                            if (aa == '/')
                            {
                                break;
                            }
                            else if (aa == '\\')
                            {
                                Write(aa);
                                aa = Get();
                            }
                            else if (aa <= '\n')
                            {
                                throw new Exception("Unterminated Regular Expression literal.");
                            }
                            Write(aa);
                        }
                        bb = Next();
                    }
                    break;
            }
        }


        private void Write(int ch)
        {
            writer.Write((char) ch);
        }

        /* IsAlphanum -- return true if the character is a letter, digit, underscore,
                dollar sign, or non-ASCII character.
        */

        private static bool IsAlphanum(int ch)
        {
            return ((ch >= 'a' && ch <= 'z') || 
                (ch >= '0' && ch <= '9') ||
                (ch >= 'A' && ch <= 'Z') || 
                ch == '_' || ch == '$' || ch == '\\' || ch > 126);
        }

        [ Serializable ]
            public class Exception : System.Exception
        {
            public Exception() {}

            public Exception(string message) : 
                base(message) {}

            public Exception(string message, Exception innerException) :
                base(message, innerException) {}

            protected Exception(SerializationInfo info, StreamingContext context) :
                base(info, context) {}
        }
    }
}