using System;
using BBCOMM = Bloomberglp.Blpapi;
using Bloomberglp;
using System.Collections.Generic;

namespace Bloomberg
{
    //Abstract Class for data request
    public abstract class BBCOMDataRequest
    {
        //BBCOM Names
        protected readonly BBCOMM.Name SECURITY_DATA = new BBCOMM.Name("securityData");
        protected readonly BBCOMM.Name FIELD_DATA = new BBCOMM.Name("fieldData");
        protected readonly BBCOMM.Name FIELD_ID = new BBCOMM.Name("fieldId");
        protected readonly BBCOMM.Name VALUE = new BBCOMM.Name("value");
        protected readonly BBCOMM.Name OVERRIDES = new BBCOMM.Name("overrides");
        protected readonly BBCOMM.Name SECURITIES = new BBCOMM.Name("securities");
        protected readonly BBCOMM.Name SECURITY = new BBCOMM.Name("security");
        protected readonly BBCOMM.Name FIELDS = new BBCOMM.Name("fields");
        protected readonly BBCOMM.Name SEQUENCE_NUMBER = new BBCOMM.Name("sequenceNumber");
        protected readonly BBCOMM.Name START_DATE = new BBCOMM.Name("startDate");
        protected readonly BBCOMM.Name END_DATE = new BBCOMM.Name("endDate");
        protected readonly BBCOMM.Name START_DATE_TIME = new BBCOMM.Name("startDateTime");
        protected readonly BBCOMM.Name END_DATE_TIME = new BBCOMM.Name("endDateTime");
        protected readonly BBCOMM.Name DATE = new BBCOMM.Name("date");
        protected readonly BBCOMM.Name OVERRIDE_CURRENCY = new BBCOMM.Name("currency");
        protected readonly BBCOMM.Name ADJUSTMENTNORMAL = new BBCOMM.Name("adjustmentNormal");
        protected readonly BBCOMM.Name PERIODICITYADJUSTEMENT = new BBCOMM.Name("periodicityAdjustment");
        protected readonly BBCOMM.Name PERIODICITYSELECTION = new BBCOMM.Name("periodicitySelection");
        protected readonly BBCOMM.Name NONTRADINGDAYFILLOPTION = new BBCOMM.Name("nonTradingDayFillOption");
        protected readonly BBCOMM.Name NONTRADINGDAYFILLMETHOD = new BBCOMM.Name("nonTradingDayFillMethod");
        protected readonly BBCOMM.Name RETURNRELATIVEDATE = new BBCOMM.Name("returnRelativeDate");
        protected readonly BBCOMM.Name RETURNEIDS = new BBCOMM.Name("returnEids");
        protected readonly BBCOMM.Name ADJUSTEMENTNORMAL = new BBCOMM.Name("adjustmentNormal");
        protected readonly BBCOMM.Name ADJUSTEMENTABNORMAL = new BBCOMM.Name("adjustmentAbnormal");
        protected readonly BBCOMM.Name ADJUSTMENTSPLIT = new BBCOMM.Name("adjustmentSplit");




        //Const String, enumators, etc..
        protected readonly string NOT_AVAILABLE = null;
        protected readonly string SESSION_EXCEPTION = "Session not started";
        protected readonly string SERVICE_EXCEPTION = "Service not opened";
        protected readonly string REQUEST_TYPE_REFERENCE = "ReferenceDataRequest";
        protected readonly string REQUEST_TYPE_HISTORICAL = "HistoricalDataRequest";
        protected readonly string REQUEST_TYPE_TICK = "IntradayTickRequest";
        protected readonly string REFERENCE_DATA_SERVICE = "//blp/refdata";
        protected readonly string BLOOMBERG_DATE_FORMAT = "yyyyMMdd";
        protected string requestType;
        protected string startDate;
        protected string endDate;
        protected string overrideCurrency;

        //wrapped BBCOMM Objects
        protected BBCOMM.Session session;
        protected BBCOMM.Service service;
        protected BBCOMM.Request request;

        //Input Data Structure
        protected List<string> securityNames = new List<string>();
        protected List<string> fieldNames = new List<string>();
        protected List<string> overrideFields = new List<string>();
        protected List<string> overrideValues = new List<string>();
        protected Boolean AdjustedHistoricalData;

        //Outout Data Structure
        protected dynamic[,,] result;

        //Process Data Full 
        public dynamic[,,] Process_Full()
        {
            Open();
            CreateRequest();
            SendRequest();
            Close();
            return result;
        }

        public dynamic[,,] Process()
        {
            CreateRequest();
            SendRequest();
            return result;
        }

        //Open Bloomberg Session on local instance
        public void Open()
        {
            // create and start bloomberg BBCOMM session
            BBCOMM.SessionOptions sessionOptions = new BBCOMM.SessionOptions();
            session = new BBCOMM.Session(sessionOptions);
            if (!session.Start()) throw new Exception(SESSION_EXCEPTION);
            //
            // get service from session object and create request by service object
            if (!session.OpenService(REFERENCE_DATA_SERVICE)) throw new Exception(SERVICE_EXCEPTION);
            service = session.GetService(REFERENCE_DATA_SERVICE);
            request = service.CreateRequest(requestType);
        }

        //Open Bloomberg Session on a distance instance
        public void Open(string Server_Host, int Server_Port)
        {
            // create and start bloomberg BBCOMM session
            BBCOMM.SessionOptions sessionOptions = new BBCOMM.SessionOptions();
            sessionOptions.ServerHost = Server_Host;
            sessionOptions.ServerPort = Server_Port;
            session = new BBCOMM.Session(sessionOptions);
            if (!session.Start()) throw new Exception(SESSION_EXCEPTION);
            //
            // get service from session object and create request by service object
            if (!session.OpenService(REFERENCE_DATA_SERVICE)) throw new Exception(SERVICE_EXCEPTION);
            service = session.GetService(REFERENCE_DATA_SERVICE);
            request = service.CreateRequest(requestType);
        }

        //Create request to be send to the BBCOM Server
        public void CreateRequest()
        {
            // append securities, fields
            if(requestType == REQUEST_TYPE_TICK)
            {
                request.Set(SECURITY, securityNames[0]);
                DateTime MyStart = Convert.ToDateTime(startDate);
                BBCOMM.Datetime RealStartDate = new BBCOMM.Datetime(MyStart.Year, MyStart.Month, MyStart.Day,MyStart.Hour,MyStart.Second,0,0) ;
                request.Set(START_DATE_TIME, RealStartDate);
                DateTime MyEnd = Convert.ToDateTime(endDate);
                BBCOMM.Datetime RealEndDate = new BBCOMM.Datetime(MyEnd.Year, MyEnd.Month, MyEnd.Day, MyEnd.Hour, MyEnd.Second, 0, 0);
                request.Set(END_DATE_TIME, RealEndDate);
                request.Append("eventTypes", "TRADE");
                request.Set("includeNonPlottableEvents", true);


            }
            else { 
                foreach (string securityName in securityNames) request.Append(SECURITIES, securityName);
                foreach (string fieldName in fieldNames) request.Append(FIELDS, fieldName);
            
                //
                // conditionally, append overrides into request object
                if (overrideFields.Count > 0)
                {
                    BBCOMM.Element requestOverrides = request.GetElement(OVERRIDES);
                    for (int i = 0; i < overrideFields.Count; i++)
                    {
                       BBCOMM.Element requestOverride = requestOverrides.AppendElement();
                       requestOverride.SetElement(FIELD_ID, overrideFields[i]);
                       requestOverride.SetElement(VALUE, overrideValues[i]);

                    }
                }
            }
            // set optional parameters for historical data request
            if (requestType == REQUEST_TYPE_HISTORICAL)
            {
                request.Set(START_DATE, startDate);
                request.Set(END_DATE, endDate);
                if (overrideCurrency != String.Empty) request.Set(OVERRIDE_CURRENCY, overrideCurrency);


                //For All Request
                request.Set(PERIODICITYADJUSTEMENT, "ACTUAL");
                request.Set(PERIODICITYSELECTION, "DAILY");
                request.Set(NONTRADINGDAYFILLOPTION, "ACTIVE_DAYS_ONLY");
                request.Set(NONTRADINGDAYFILLMETHOD, "NIL_VALUE");
                request.Set(RETURNRELATIVEDATE, false);
                request.Set(RETURNEIDS, false);

                if (AdjustedHistoricalData == true)
                {
                    request.Set(ADJUSTEMENTNORMAL, true);
                    request.Set(ADJUSTEMENTABNORMAL, true);
                    request.Set(ADJUSTMENTSPLIT, true);
                }
                else
                {
                    if (AdjustedHistoricalData == false)
                    {
                        //Not Sure if OK but seems to raise error if split is not adjsuted
                        request.Set(ADJUSTEMENTNORMAL, false);
                        request.Set(ADJUSTEMENTABNORMAL, false);
                        request.Set(ADJUSTMENTSPLIT, true);
                    }
                }
            }

        }

        //Send the Request to the BBCOM Server
        public void SendRequest()
        {
            // send constructed request to BBCOMM server
            long ID = Guid.NewGuid().GetHashCode();
            session.SendRequest(request, new BBCOMM.CorrelationID(ID));
            bool isProcessing = true;
            //
            while (isProcessing)
            {
                // receive data response from BBCOMM server, send 
                // response to be processed by sub-classed algorithm
                BBCOMM.Event response = session.NextEvent();
                switch (response.Type)
                {
                    case BBCOMM.Event.EventType.PARTIAL_RESPONSE:
                        ProcessDataResponse(ref response);
                        break;
                    case BBCOMM.Event.EventType.RESPONSE:
                        ProcessDataResponse(ref response);
                        isProcessing = false;
                        break;
                    default:
                        break;
                }
            }
        }

        //Close the BBCOM Session
        public void Close()
        {
            // close BBCOMM session
            if (session != null) session.Stop();
        }

        // sub-classes are providing specific algorithm implementations for 
        // processing and packing BBCOMM server response data into resulting data structure
        protected abstract void ProcessDataResponse(ref BBCOMM.Event response);
    }

    // concrete class implementation for processing reference data request
    public class ReferenceDataRequest : BBCOMDataRequest
    {
        //Reference Data Request Without Overides Values
        public ReferenceDataRequest(List<string> bloombergSecurityNames,
            List<string> bloombergFieldNames)
        {
            // ctor : create reference data request without field overrides
            requestType = REQUEST_TYPE_REFERENCE;
            securityNames = bloombergSecurityNames;
            fieldNames = bloombergFieldNames;
            //
            // define result data structure dimensions for reference data request
            result = new dynamic[securityNames.Count, 1, fieldNames.Count];
        }

        //Reference Data Request with Overides Values
        public ReferenceDataRequest(List<string> bloombergSecurityNames,
            List<string> bloombergFieldNames, List<string> bloombergOverrideFields,
            List<string> bloombergOverrideValues)
        {
            // ctor : create reference data request with field overrides
            requestType = REQUEST_TYPE_REFERENCE;
            securityNames = bloombergSecurityNames;
            fieldNames = bloombergFieldNames;
            overrideFields = bloombergOverrideFields;
            overrideValues = bloombergOverrideValues;
            //
            // define result data structure dimensions for reference data request
            result = new dynamic[securityNames.Count, 1, fieldNames.Count];
        }

        //Process the data Response
        protected override void ProcessDataResponse(ref BBCOMM.Event response)
        {
            // receive response, which contains N securities and M fields
            // event queue can send multiple responses for large requests
            foreach (BBCOMM.Message message in response.GetMessages())
            {
                // extract N securities
                BBCOMM.Element securities = message.GetElement(SECURITY_DATA);
                int nSecurities = securities.NumValues;
                //
                // loop through all securities
                for (int i = 0; i < nSecurities; i++)
                {
                    // extract one security and fields for this security
                    BBCOMM.Element security = securities.GetValueAsElement(i);
                    BBCOMM.Element fields = security.GetElement(FIELD_DATA);
                    int sequenceNumber = security.GetElementAsInt32(SEQUENCE_NUMBER);
                    int nFieldNames = fieldNames.Count;
                    //
                    // loop through all M fields for this security
                    for (int j = 0; j < nFieldNames; j++)
                    {
                        // if the requested field has been found, pack value into result data structure
                        if (fields.HasElement(fieldNames[j]))
                        {
                            if (fields.GetElement(fieldNames[j]).GetValue().GetType() == typeof(Char))
                            {
                                if ((char)fields.GetElement(fieldNames[j]).GetValue() == 'N')
                                {
                                    result[sequenceNumber, 0, j] = false;
                                }
                                else
                                {
                                    if ((char)fields.GetElement(fieldNames[j]).GetValue() == 'Y')
                                    {
                                        result[sequenceNumber, 0, j] = true;
                                    }

                                }

                            }
                            else
                            {
                                if (fields.GetElement(fieldNames[j]).GetValue().GetType() == typeof(string))
                                {
                                    if ((string)fields.GetElement(fieldNames[j]).GetValue() == "Y")
                                    {
                                        result[sequenceNumber, 0, j] = true;
                                    }
                                    else
                                    {
                                        if ((string)fields.GetElement(fieldNames[j]).GetValue() == "N")
                                        {
                                            result[sequenceNumber, 0, j] = false;
                                        }
                                        else
                                        {
                                            if ((string)fields.GetElement(fieldNames[j]).GetValue() == "Unknown")
                                            {
                                                result[sequenceNumber, 0, j] = null;
                                            }
                                            else
                                            {
                                                result[sequenceNumber, 0, j] = fields.GetElement(fieldNames[j]).GetValue();

                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    result[sequenceNumber, 0, j] = fields.GetElement(fieldNames[j]).GetValue();
                                }
                            }
                        }
                        // otherwise, pack NOT_AVAILABLE string into data structure
                        else
                        {
                            result[sequenceNumber, 0, j] = NOT_AVAILABLE;
                        }
                    }
                }
            }
        }

    }

    //Concrete Class implementation for processing historical data request
    public class HistoricalDataRequest : BBCOMDataRequest
    {
        private bool hasDimensions = false;
        // create historical data request without field overrides
        public HistoricalDataRequest(List<string> bloombergSecurityNames, List<string> bloombergFieldNames, DateTime bloombergStartDate, DateTime BloombergEndDate, string bloombergOverrideCurrency = "")
        {

            requestType = REQUEST_TYPE_HISTORICAL;
            securityNames = bloombergSecurityNames;
            fieldNames = bloombergFieldNames;
            startDate = bloombergStartDate.ToString(BLOOMBERG_DATE_FORMAT);
            endDate = BloombergEndDate.ToString(BLOOMBERG_DATE_FORMAT);
            overrideCurrency = bloombergOverrideCurrency;
        }
        //Create historical data request with overide fields
        public HistoricalDataRequest(List<string> bloombergSecurityNames, List<string> bloombergFieldNames,
            DateTime bloombergStartDate, DateTime BloombergEndDate, List<string> bloombergOverrideFields,
            List<string> bloombergOverrideValues, string bloombergOverrideCurrency = "")
        {
            // ctor : create historical data request with field overrides
            requestType = REQUEST_TYPE_HISTORICAL;
            securityNames = bloombergSecurityNames;
            fieldNames = bloombergFieldNames;
            overrideFields = bloombergOverrideFields;
            overrideValues = bloombergOverrideValues;
            startDate = bloombergStartDate.ToString(BLOOMBERG_DATE_FORMAT);
            endDate = BloombergEndDate.ToString(BLOOMBERG_DATE_FORMAT);
            overrideCurrency = bloombergOverrideCurrency;
        }

        public HistoricalDataRequest(List<string> bloombergSecurityNames, List<string> bloombergFieldNames,
    DateTime bloombergStartDate, DateTime BloombergEndDate, Boolean AdjustedData)
        {
            // ctor : create historical data request with field overrides
            requestType = REQUEST_TYPE_HISTORICAL;
            securityNames = bloombergSecurityNames;
            fieldNames = bloombergFieldNames;
            startDate = bloombergStartDate.ToString(BLOOMBERG_DATE_FORMAT);
            endDate = BloombergEndDate.ToString(BLOOMBERG_DATE_FORMAT);
            AdjustedHistoricalData = AdjustedData;
        }
        protected override void ProcessDataResponse(ref BBCOMM.Event response)
        {
            // unzip and pack messages received from BBCOMM server
            // receive one security per message and multiple messages per event
            foreach (BBCOMM.Message message in response.GetMessages())
            {
                // extract security and fields
                BBCOMM.Element security = message.GetElement(SECURITY_DATA);
                BBCOMM.Element fields = security.GetElement(FIELD_DATA);
                //
                int sequenceNumber = security.GetElementAsInt32(SEQUENCE_NUMBER);
                int nFieldNames = fieldNames.Count;
                int nObservationDates = fields.NumValues;
                //
                // the exact dimension will be known only, when the response has been received from BBCOMM server
                if (!hasDimensions)
                {
                    // define result data structure dimensions for historical data request
                    // observation date will be stored into first field for each observation date
                    result = new dynamic[securityNames.Count, nObservationDates, fieldNames.Count + 1];
                    hasDimensions = true;
                }
                //
                // loop through all observation dates
                for (int i = 0; i < nObservationDates; i++)
                {
                    // extract all field data for a single observation date
                    BBCOMM.Element observationDateFields = fields.GetValueAsElement(i);
                    //
                    // pack observation date into data structure
                    result[sequenceNumber, i, 0] = observationDateFields.GetElementAsDatetime(DATE).ToSystemDateTime();
                    //
                    // then, loop through all 'user-requested' fields for a given observation date
                    // and pack results data into data structure
                    for (int j = 0; j < nFieldNames; j++)
                    {
                        // pack field value into data structure if such value has been found
                        if (observationDateFields.HasElement(fieldNames[j]))
                        {
                            result[sequenceNumber, i, j + 1] = observationDateFields.GetElement(fieldNames[j]).GetValue();
                        }
                        // otherwise, pack NOT_AVAILABLE string into data structure
                        else
                        {
                            result[sequenceNumber, i, j + 1] = NOT_AVAILABLE;
                        }
                    }
                }
            }
        }
    }

    //Concrete class implementation for processing Bulk data request
    //Caution : Bulk data request take only one ticker and one field as input 
    //although the implementation is the same than reference data requets and historical data request
    public class BulkDataRequest : BBCOMDataRequest
    {

        //BulkData Request with overide Fields
        public BulkDataRequest(List<string> bloombergSecurityNames, string bloombergFieldNames)
        {
            // ctor : create reference data request without field overrides
            requestType = REQUEST_TYPE_REFERENCE;
            securityNames = bloombergSecurityNames;
            fieldNames.Add(bloombergFieldNames);

        }

        //Reference Data Request with Overides Values
        public BulkDataRequest(List<string> bloombergSecurityNames, string bloombergFieldNames, List<string> bloombergOverrideFields, List<string> bloombergOverrideValues)
        {
            requestType = REQUEST_TYPE_REFERENCE;
            securityNames = bloombergSecurityNames;
            fieldNames.Add(bloombergFieldNames);
            overrideFields = bloombergOverrideFields;
            overrideValues = bloombergOverrideValues;

        }

        //Process the data response 
        protected override void ProcessDataResponse(ref BBCOMM.Event response)
        {
            // unzip and pack messages received from BBCOMM server
            // receive one security per message and multiple messages per event
            foreach (BBCOMM.Message message in response.GetMessages())
            {
                int num_securities = message.GetElement(SECURITY_DATA).NumValues;
                for (int i = 0; i <= num_securities - 1; i++)
                {
                    // extract security and fields
                    BBCOMM.Element security = (BBCOMM.Element)message.GetElement(SECURITY_DATA).GetValue(i);
                    BBCOMM.Element fields = security.GetElement("fieldData");
                    int num_field = fields.NumElements;
                    for (int a = 0; a <= num_field - 1; a++)
                    {
                        BBCOMM.Element Field = fields.GetElement(a);
                        int num_bulkvalues = Field.NumValues;
                        result = new dynamic[num_bulkvalues, 2, 1];
                        for (int ind1 = 0; ind1 <= num_bulkvalues - 1; ind1++)
                        {
                            BBCOMM.Element Bulk_element = (BBCOMM.Element)Field.GetValue(ind1);
                            int num_BulkElement = Bulk_element.NumElements;
                            for (int ind2 = 0; ind2 <= num_BulkElement - 1; ind2++)
                            {
                                BBCOMM.Element Element = (BBCOMM.Element)Bulk_element.GetElement(ind2);
                                if (ind2 % 2 == 0)
                                {
                                    result[ind1, 0, 0] = Element.GetValue();
                                }
                                else
                                {
                                    result[ind1, 1, 0] = Element.GetValue();

                                }
                            }
                        }
                    }
                }
            }
        }
    }
    //Concrete class implementation for processing IntradayTickRequest request
    //Caution :IntradayTickRequest request take only one ticke, and only trades
    public class IntradayTickRequest : BBCOMDataRequest
    {

        // create IntradayTickRequest data request
        public IntradayTickRequest(List<string> bloombergSecurityNames, DateTime bloombergStartDate, DateTime BloombergEndDate, string bloombergOverrideCurrency = "")
        {

            requestType = REQUEST_TYPE_TICK;
            securityNames = bloombergSecurityNames;
            startDate = bloombergStartDate.ToString();
            endDate = BloombergEndDate.ToString();
            overrideCurrency = bloombergOverrideCurrency;
        }
        //Create historical data request with overide fields
     
        protected override void ProcessDataResponse(ref BBCOMM.Event response)
        {
            // unzip and pack messages received from BBCOMM server
            // receive one security per message and multiple messages per event
            foreach (BBCOMM.Message message in response.GetMessages())
            {
                // extract security and fields
               
                // the exact dimension will be known only, when the response has been received from BBCOMM server
                BBCOMM.Element elmTickDataArr = message["tickData"];
                BBCOMM.Element elmTickData = elmTickDataArr["tickData"];

                result = new dynamic[elmTickData.NumValues, 1,3];
                for (int valueCount = 0; valueCount < elmTickData.NumValues; valueCount++)
                {
                    BBCOMM.Element elmTickDataValue = elmTickData.GetValueAsElement(valueCount);

                    result[valueCount, 0, 0] = elmTickDataValue.GetElementAsDatetime("time").ToSystemDateTime();//Date du trade
                    result[valueCount, 0, 1] = elmTickDataValue.GetElementAsFloat64("value"); // Price
                    result[valueCount, 0, 2] = elmTickDataValue.GetElementAsInt32("size");//Size

                }
            }
            
        }
    }
}
