/*
    ============================================================================

    Module Name:        Program.cs

    Namespace Name:     JSON_Jam

    Class Name:         Program

    Synopsis:           This command line utility demonstrates a solution to two
                        issues that I recently encountered when processing JSON
                        strings returned as responses from various Web services.

    Remarks:            This class module implements the Program class, which is
                        composed primarily of the static void Main method,
                        which is functionally equivalent to the main() routine
                        of a standard C program.

    Author:             David A. Gray

    License:            Copyright (C) 2019, David A. Gray. 
                        All rights reserved.

                        Redistribution and use in source and binary forms, with
                        or without modification, are permitted provided that the
                        following conditions are met:

                        *   Redistributions of source code must retain the above
                            copyright notice, this list of conditions and the
                            following disclaimer.

                        *   Redistributions in binary form must reproduce the
                            above copyright notice, this list of conditions and
                            the following disclaimer in the documentation and/or
                            other materials provided with the distribution.

                        *   Neither the name of David A. Gray, nor the names of
                            his contributors may be used to endorse or promote
                            products derived from this software without specific
                            prior written permission.

                        THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
                        CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED
                        WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
                        WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
                        PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
                        David A. Gray BE LIABLE FOR ANY DIRECT, INDIRECT,
                        INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
                        (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
                        SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
                        PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
                        ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
                        LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
                        ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN
                        IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

    ----------------------------------------------------------------------------
    Revision History
    ----------------------------------------------------------------------------

    Date       Version Author Synopsis
    ---------- ------- ------ -------------------------------------------------
    2019/05/13 1.0     DAG    This is the first version.

    2019/05/22 1.1     DAG    Write the details returned in the JSON response to
                              a tab-delimited text file, log its name on the
                              console, and add the flower box and a three-clause
                              BSD license.

    2019/05/28 1.2     DAG    Clean up the flower box.
                              The code is otherwase unchanged.
    ============================================================================
*/

using System;

using System.IO;
using System.Reflection;

using WizardWrx;

namespace JSON_Jam
{
    class Program
    {
        static readonly DateTime s_dtmStart = DateTime.UtcNow;


        static void Main ( string [ ] args )
        {
            Console.WriteLine ( CreateStartupBanner ( ) );

            try
            {
                string strRawResponse = GetRawJSONString ( );
                JSONFixupEngine engine = new JSONFixupEngine ( @"TIME_SERIES_DAILY_ResponseMap" );
                string strFixedUp_Pass_1 = engine.ApplyFixups_Pass_1 ( strRawResponse );

                PreserveResult (
                    strFixedUp_Pass_1 ,                                         // string pstrPreserveThisResult
                    Properties.Settings.Default.JSON_INTERMEDIATE_FILE_NAME ,   // string pstrOutputFileNamePerSettings
                    Properties.Resources.FILE_LABEL_INTERMEDIATE );             // string pstrLabelForReportMessage

                string strFixedUp_Pass_2 = engine.ApplyFixups_Pass_2 ( strFixedUp_Pass_1 );

                PreserveResult (
                    strFixedUp_Pass_2 ,                                         // string pstrPreserveThisResult
                    Properties.Settings.Default.JSON_FINAL_FILE_NAME ,          // string pstrOutputFileNamePerSettings
                    Properties.Resources.FILE_LABEL_FINAL );                    // string pstrLabelForReportMessage

                //  ------------------------------------------------------------
                //  TimeSeriesDailyResponse
                //  ------------------------------------------------------------

                ConsumeResponse (
                    Properties.Settings.Default.JSON_CONTENTS_REPORT_FILE_NAME ,
                    Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSeriesDailyResponse> (
                        strFixedUp_Pass_2 ) );
            }
            catch ( Exception exAll )
            {
                string strMsg = exAll.ToString ( );
                Console.WriteLine ( strMsg );

                Environment.ExitCode = WizardWrx.MagicNumbers.ERROR_RUNTIME;
            }

            Console.WriteLine ( CreateShutdownBanner ( ) );
            AwaitCarbonUnit ( );
        }   // static void Main


        /// <summary>
        /// Assemble an absolute file name from the location of the executing
        /// assembly, a directory path that is specified relative to it, and the
        /// unqualified name to assign to it, specified by <paramref name="pstrFileNamePerAppSettings"/>.
        /// </summary>
        /// <param name="pstrFileNamePerAppSettings">
        /// String containing the unqualified name to assign to a file
        /// </param>
        /// <returns>
        /// The return value is the absolute name of a file that is assembled by
        /// combining the names of the directory from which the executing
        /// assembly was loaded and another directory name specified relative to
        /// it with the unqualified name specified in <paramref name="pstrFileNamePerAppSettings"/>.
        /// </returns>
        private static string AssembleAbsoluteFileName ( string pstrFileNamePerAppSettings )
        {
            string strAssemblyDirectoryName = Path.GetDirectoryName ( Assembly.GetExecutingAssembly ( ).Location );
            string strTestDataDirectoryName = Properties.Settings.Default.DATA_DIRECTORY_NAME;
            string strAbsoluteInputFileName = Path.Combine (
                new string [ ]
                {
                    strAssemblyDirectoryName,
                    strTestDataDirectoryName,
                    pstrFileNamePerAppSettings
                } );
            return strAbsoluteInputFileName;
        }   // private static string AssembleAbsoluteFileName


        /// <summary>
        /// Prompt the user to press the RETURN key to end the program, allowing
        /// it to be launched from the File Explorer.
        /// </summary>
        private static void AwaitCarbonUnit ( )
        {
            Console.Error.WriteLine ( Properties.Resources.MSG_AWAIT_CARBON_UNIT );
            Console.ReadLine ( );
        }   // private static void AwaitCarbonUnit ( )


        /// <summary>
        /// Create a report of the contents of the deserialized response in
        /// <paramref name="timeSeriesDailyResponse"/>.
        /// </summary>
        /// <param name="timeSeriesDailyResponse">
        /// The populated TimeSeriesDailyResponse instance returned by the JSON
        /// deserializer.
        /// </param>
        private static void ConsumeResponse (
            string pstrReportFileName ,
            TimeSeriesDailyResponse timeSeriesDailyResponse )
        {
            Console.WriteLine (
                Properties.Resources.MSG_RESPONSE_METADATA ,                    // Format control string
                new object [ ]
                {
                    timeSeriesDailyResponse.Meta_Data.Information ,             // Format item 0: Information   = {0}
                    timeSeriesDailyResponse.Meta_Data.Symbol ,                  // Format Item 1: Symbol        = {1}
                    timeSeriesDailyResponse.Meta_Data.LastRefreshed ,           // Format Item 2: LastRefreshed = {2}
                    timeSeriesDailyResponse.Meta_Data.OutputSize ,              // Format Item 3: OutputSize    = {3}
                    timeSeriesDailyResponse.Meta_Data.TimeZone ,                // Format Item 4: TimeZone      = {4}
                    timeSeriesDailyResponse.Time_Series_Daily.Length ,          // Format Item 5: Detail Count  = {5}
                    Environment.NewLine                                         // Format Item 6: Platform-dependent newline
                } );

            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( pstrReportFileName );

            using ( StreamWriter swTimeSeriesDetail = new StreamWriter ( strAbsoluteInputFileName ,
                                                                         FileIOFlags.FILE_OUT_CREATE ,
                                                                         System.Text.Encoding.ASCII ,
                                                                         MagicNumbers.CAPACITY_08KB ) )
            {
                string strLabelRow = Properties.Resources.MSG_RESPONSE_DETAILS_LABELS.ReplaceEscapedTabsInStringFromResX ( );
                swTimeSeriesDetail.WriteLine ( strLabelRow );
                string strDetailRowFormatString = ReportHelpers.DetailTemplateFromLabels ( strLabelRow );

                for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
                          intJ < timeSeriesDailyResponse.Time_Series_Daily.Length ;
                          intJ++ )
                {
                    Time_Series_Daily daily = timeSeriesDailyResponse.Time_Series_Daily [ intJ ];
                    swTimeSeriesDetail.WriteLine (
                        strDetailRowFormatString ,
                        new object [ ]
                        {
                            ArrayInfo.OrdinalFromIndex ( intJ ) ,               // Format Item 0: Item
                            Beautify ( daily.Activity_Date) ,                   // Format Item 1: Activity_Date
                            Beautify ( daily.Open ) ,                           // Format Item 2: Open
                            Beautify ( daily.High ) ,                           // Format Item 3: High
                            Beautify ( daily.Low ) ,                            // Format Item 4: Low
                            Beautify ( daily.Close ) ,                          // Format Item 5: Close
                            Beautify ( daily.AdjustedClose ) ,                  // Format Item 6: AdjustedClose
                            Beautify ( daily.Volume ) ,                         // Format Item 7: Volume
                            Beautify ( daily.DividendAmount ) ,                 // Format Item 8: DividendAmount
                            Beautify ( daily.SplitCoefficient )                 // Format Item 9: SplitCoefficient
                        } );
                }   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < timeSeriesDailyResponse.Time_Series_Daily.Length ; intJ++ )
            }   // using ( StreamWriter swTimeSeriesDetail = new StreamWriter ( strAbsoluteInputFileName , FileIOFlags.FILE_OUT_CREATE , System.Text.Encoding.ASCII , MagicNumbers.CAPACITY_08KB ) )

            Console.WriteLine (
                ShowFileDetails (                                               // Print the returned string.
                    Properties.Resources.FILE_LABEL_CONTENT_REPORT ,            // string pstrLabel
                    strAbsoluteInputFileName ) );                               // string pstrFileName
        }   // private static void ConsumeResponse


        /// <summary>
        /// Format date/time and integral types for printing, passing all other
        /// types through as is, by way of their ToString methods.
        /// </summary>
        /// <param name="pstrStringFromJSON">
        /// The string from the deserialized JSON object is converted to a safer
        /// type, then fed through its ToString method.
        /// </param>
        /// <returns>
        /// The return value is a formatted string, suitable for display on a
        /// report.
        /// </returns>
        internal static string Beautify ( string pstrStringFromJSON )
        {
            object objOfType = ConvertToAppropriateType ( pstrStringFromJSON );

            if ( objOfType is DateTime )
            {
                DateTime dtmObjAsDate = ( DateTime ) objOfType;
                return SysDateFormatters.ReformatSysDate ( dtmObjAsDate , SysDateFormatters.RFD_YYYY_MM_DD );
            }   // TRUE (The input is a DateTime.) block, if ( objOfType is DateTime )
            else if ( objOfType is long )
            {
                long lngObjAsLongInteger = ( long ) objOfType;
                return lngObjAsLongInteger.ToString ( NumericFormats.NUMBER_PER_REG_SETTINGS_0D );
            }   // TRUE (The input is a Long Integer.) block, else if ( objOfType is long )
            else
            {
                return objOfType.ToString ( );
            }   // FALSE block covering if ( objOfType is DateTime ) AND else if ( objOfType is long )
        }   // internal static string Beautify


        /// <summary>
        /// Convert a string to the type that most faithfully represents its
        /// contents.
        /// </summary>
        /// <param name="pstrStringFromJSON">
        /// String to convert
        /// </param>
        /// <returns>
        /// Contents of <paramref name="pstrStringFromJSON"/> parsed into the
        /// most appropriate representation from among DateTime, Integer, and
        /// Double, in that order of preference
        /// </returns>
        internal static object ConvertToAppropriateType ( string pstrStringFromJSON )
        {
            DateTime dtmTemp;

            if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
            {
                return dtmTemp;
            }   // TRUE (Input value is a DateTime.) block, if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
            else
            {
                long lngTemp;

                if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
                {
                    return lngTemp;
                }   // TRUE (Input value is a Long Integer.) block, if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
                else
                {
                    double dblTemp;

                    if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                    {
                        return dblTemp;
                    }   // TRUE (Input value is a Double Precision floating point number.) block, if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                    else
                    {
                        return pstrStringFromJSON;
                    }   // FALSE (Input value is of another type) block, if ( double.TryParse ( pstrStringFromJSON , out dblTemp ) )
                }   // FALSE (Input value is of another type.) block, if ( long.TryParse ( pstrStringFromJSON , out lngTemp ) )
            }   // FALSE (Input value is of another type.) block, if ( DateTime.TryParse ( pstrStringFromJSON , out dtmTemp ) )
        }   // internal static object ConvertToAppropriateType


        /// <summary>
        /// Generate a shutdown message that reports the name of the calling
        /// program, the current time per the system clock, and the wall clock
        /// time consumed by the program.
        /// </summary>
        /// <returns>
        /// The return value is a string that can be displayed on the console or
        /// recorded in a log file.
        /// </returns>
        private static string CreateShutdownBanner ( )
        {
            AssemblyName anTheApp = Assembly.GetEntryAssembly ( ).GetName ( );
            DateTime dtmStopping = DateTime.UtcNow;
            TimeSpan tsRunning = dtmStopping - s_dtmStart;

            return string.Format (
                Properties.Resources.MSG_STOP ,             // The format control string contains five substitution tokens.
                new object [ ]								// Since there are more than three, the format items go into a parameter array.
				{
                    anTheApp.Name ,							// Format Item 0 = Program Name
					dtmStopping.ToLocalTime ( ) ,			// Format Item 1 = Local Program Ending Time
					dtmStopping ,							// Format Item 2 = UTC Program Ending Time
					tsRunning ,								// Format Item 3 = Running time
					Environment.NewLine } );                // Format Item 4 = Embedded Newline
        }   // private static string CreateShutdownBanner ( )


        /// <summary>
        /// Create a startup message that reports the name and version (major
        /// and minor) of the calling program and the current time per the
        /// system clock.
        /// </summary>
        /// <returns>
        /// The return value is a string that can be displayed on the console or
        /// recorded in a log file.
        /// </returns>
        private static string CreateStartupBanner ( )
        {
            AssemblyName anTheApp = Assembly.GetEntryAssembly ( ).GetName ( );

            return string.Format (
                Properties.Resources.MSG_START ,            // The format control string contains six substitution tokens.
                new object [ ]								// Since there are more than three, the format items go into a parameter array.
				{
                    anTheApp.Name ,							// Format Item 0 = Program Name
					anTheApp.Version.Major ,				// Format Item 1 = Major Version Number
					anTheApp.Version.Minor ,				// Format Item 2 = Minor Version Number
					s_dtmStart.ToLocalTime ( ) ,			// Format Item 3 = Local Startup Time
					s_dtmStart ,							// Format Item 4 = UTC Startup Time
					Environment.NewLine						// Format Item 5 = Embedded Newline
				} );
        }   // private static string CreateStartupBanner


        /// <summary>
        /// Read the raw JSON string from a text file, which is expected to be
        /// stored in a text file which is fully qualified by application config
        /// values JSON_INPUT_FILE_NAME and DATA_DIRECTORY_NAME.
        /// </summary>
        /// <returns>
        /// If the method succeeds, the return value is the sample JSON string
        /// to be converted. Since it can throw an I/O exception, this method
        /// must be called from whthin a Try block.
        /// </returns>
        private static string GetRawJSONString ( )
        {
            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( Properties.Settings.Default.JSON_INPUT_FILE_NAME );

            Console.WriteLine (
                ShowFileDetails (
                    Properties.Resources.FILE_LABEL_INPUT ,                     // string pstrLabel
                    strAbsoluteInputFileName ) );                               // string pstrFileName
            return File.ReadAllText ( strAbsoluteInputFileName );               // string pstrEndOfLastLine = SpecialStrings.EMPTY_STRING )
        }   // private static string GetRawJSONString


        /// <summary>
        ///
        /// </summary>
        /// <param name="pstrPreserveThisResult">
        /// String containing text to be preserved in the file identified by
        /// <paramref name="pstrOutputFileNamePerSettings"/>
        /// </param>
        /// <param name="pstrOutputFileNamePerSettings">
        /// String containing the unqualified name to give to the file into
        /// which the string specified by <paramref name="pstrPreserveThisResult"/>
        /// should be written
        /// </param>
        /// <param name="pstrLabelForReportMessage">
        /// String containing the label to use as the prefix on the report about
        /// the output file specified by <paramref name="pstrOutputFileNamePerSettings"/>
        /// after the string specified by <paramref name="pstrLabelForReportMessage"/>
        /// is written into it
        /// </param>
        private static void PreserveResult (
            string pstrPreserveThisResult ,
            string pstrOutputFileNamePerSettings ,
            string pstrLabelForReportMessage )
        {
            string strAbsoluteInputFileName = AssembleAbsoluteFileName ( pstrOutputFileNamePerSettings );

            File.WriteAllText ( strAbsoluteInputFileName , pstrPreserveThisResult );

            Console.WriteLine (
                ShowFileDetails (
                    pstrLabelForReportMessage ,                                 // string pstrLabel
                    strAbsoluteInputFileName ) );                               // string pstrFileName
        }   // private static void PreserveResult


        /// <summary>
        /// Use a FileInfo object to assemble and report details about a file.
        /// </summary>
        /// <param name="pstrLabel">
        /// Label for report about file <paramref name="pstrFileName"/>
        /// </param>
        /// <param name="pstrFileName">
        /// File about which to compose report labeled per <paramref name="pstrLabel"/>
        /// </param>
        /// <param name="pstrEndOfLastLine">
        /// Optional termination string for report, defaulting to the empty
        /// string, but overrideable with e. g., a newline to follow the report
        /// with a blank line
        /// </param>
        /// <returns>
        /// String for use as a console or log message
        /// </returns>
        private static string ShowFileDetails (
            string pstrLabel ,
            string pstrFileName )
        {
            FileInfo info = new FileInfo ( pstrFileName );
            return info.ShowFileDetails (
                FileInfoExtensionMethods.FileDetailsToShow.Everything ,
                pstrLabel ,
                false ,
                true );
        }   // private static string ShowFileDetails
    }   // class Program
}   // partial namespace JSON_Jam