﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Unito.EUCases.CrawlersUploader.ServiceEUCases {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UploadDocumentGroup", Namespace="http://schemas.datacontract.org/2004/07/ServiceEUCases")]
    [System.SerializableAttribute()]
    public partial class UploadDocumentGroup : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MetaInfoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MetaInfo {
            get {
                return this.MetaInfoField;
            }
            set {
                if ((object.ReferenceEquals(this.MetaInfoField, value) != true)) {
                    this.MetaInfoField = value;
                    this.RaisePropertyChanged("MetaInfo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UploadDocumentGroupComplex", Namespace="http://schemas.datacontract.org/2004/07/ServiceEUCases")]
    [System.SerializableAttribute()]
    public partial class UploadDocumentGroupComplex : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Unito.EUCases.CrawlersUploader.ServiceEUCases.XmlDocumentGroup MetaInfoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Unito.EUCases.CrawlersUploader.ServiceEUCases.XmlDocumentGroup MetaInfo {
            get {
                return this.MetaInfoField;
            }
            set {
                if ((object.ReferenceEquals(this.MetaInfoField, value) != true)) {
                    this.MetaInfoField = value;
                    this.RaisePropertyChanged("MetaInfo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="XmlDocumentGroup", Namespace="http://schemas.datacontract.org/2004/07/MetainfoEUCases")]
    [System.SerializableAttribute()]
    public partial class XmlDocumentGroup : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string CrawlerField;
        
        private string DateField;
        
        private Unito.EUCases.CrawlersUploader.ServiceEUCases.XmlDocumentMetaInfo[] DocumentField;
        
        private string FileNameField;
        
        private string FormatField;
        
        private string IdentifierField;
        
        private string LangField;
        
        private Unito.EUCases.CrawlersUploader.ServiceEUCases.Operation OperationField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Crawler {
            get {
                return this.CrawlerField;
            }
            set {
                if ((object.ReferenceEquals(this.CrawlerField, value) != true)) {
                    this.CrawlerField = value;
                    this.RaisePropertyChanged("Crawler");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Date {
            get {
                return this.DateField;
            }
            set {
                if ((object.ReferenceEquals(this.DateField, value) != true)) {
                    this.DateField = value;
                    this.RaisePropertyChanged("Date");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public Unito.EUCases.CrawlersUploader.ServiceEUCases.XmlDocumentMetaInfo[] Document {
            get {
                return this.DocumentField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentField, value) != true)) {
                    this.DocumentField = value;
                    this.RaisePropertyChanged("Document");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Format {
            get {
                return this.FormatField;
            }
            set {
                if ((object.ReferenceEquals(this.FormatField, value) != true)) {
                    this.FormatField = value;
                    this.RaisePropertyChanged("Format");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Identifier {
            get {
                return this.IdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.IdentifierField, value) != true)) {
                    this.IdentifierField = value;
                    this.RaisePropertyChanged("Identifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Lang {
            get {
                return this.LangField;
            }
            set {
                if ((object.ReferenceEquals(this.LangField, value) != true)) {
                    this.LangField = value;
                    this.RaisePropertyChanged("Lang");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public Unito.EUCases.CrawlersUploader.ServiceEUCases.Operation Operation {
            get {
                return this.OperationField;
            }
            set {
                if ((this.OperationField.Equals(value) != true)) {
                    this.OperationField = value;
                    this.RaisePropertyChanged("Operation");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="XmlDocumentMetaInfo", Namespace="http://schemas.datacontract.org/2004/07/MetainfoEUCases")]
    [System.SerializableAttribute()]
    public partial class XmlDocumentMetaInfo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string FileField;
        
        private string FormatField;
        
        private string IdentifierField;
        
        private string Md5Field;
        
        private Unito.EUCases.CrawlersUploader.ServiceEUCases.Operation OperationField;
        
        private string UrlField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string File {
            get {
                return this.FileField;
            }
            set {
                if ((object.ReferenceEquals(this.FileField, value) != true)) {
                    this.FileField = value;
                    this.RaisePropertyChanged("File");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Format {
            get {
                return this.FormatField;
            }
            set {
                if ((object.ReferenceEquals(this.FormatField, value) != true)) {
                    this.FormatField = value;
                    this.RaisePropertyChanged("Format");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Identifier {
            get {
                return this.IdentifierField;
            }
            set {
                if ((object.ReferenceEquals(this.IdentifierField, value) != true)) {
                    this.IdentifierField = value;
                    this.RaisePropertyChanged("Identifier");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Md5 {
            get {
                return this.Md5Field;
            }
            set {
                if ((object.ReferenceEquals(this.Md5Field, value) != true)) {
                    this.Md5Field = value;
                    this.RaisePropertyChanged("Md5");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public Unito.EUCases.CrawlersUploader.ServiceEUCases.Operation Operation {
            get {
                return this.OperationField;
            }
            set {
                if ((this.OperationField.Equals(value) != true)) {
                    this.OperationField = value;
                    this.RaisePropertyChanged("Operation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public string Url {
            get {
                return this.UrlField;
            }
            set {
                if ((object.ReferenceEquals(this.UrlField, value) != true)) {
                    this.UrlField = value;
                    this.RaisePropertyChanged("Url");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Operation", Namespace="http://schemas.datacontract.org/2004/07/MetainfoEUCases")]
    public enum Operation : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Add = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Upd = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Del = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceEUCases.IServiceEUCases")]
    public interface IServiceEUCases {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEUCases/UploadFile", ReplyAction="http://tempuri.org/IServiceEUCases/UploadFileResponse")]
        string UploadFile(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroup document);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEUCases/UploadFile", ReplyAction="http://tempuri.org/IServiceEUCases/UploadFileResponse")]
        System.Threading.Tasks.Task<string> UploadFileAsync(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroup document);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEUCases/UploadFileComplex", ReplyAction="http://tempuri.org/IServiceEUCases/UploadFileComplexResponse")]
        string UploadFileComplex(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroupComplex document);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEUCases/UploadFileComplex", ReplyAction="http://tempuri.org/IServiceEUCases/UploadFileComplexResponse")]
        System.Threading.Tasks.Task<string> UploadFileComplexAsync(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroupComplex document);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceEUCasesChannel : Unito.EUCases.CrawlersUploader.ServiceEUCases.IServiceEUCases, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceEUCasesClient : System.ServiceModel.ClientBase<Unito.EUCases.CrawlersUploader.ServiceEUCases.IServiceEUCases>, Unito.EUCases.CrawlersUploader.ServiceEUCases.IServiceEUCases {
        
        public ServiceEUCasesClient() {
        }
        
        public ServiceEUCasesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceEUCasesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceEUCasesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceEUCasesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string UploadFile(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroup document) {
            return base.Channel.UploadFile(document);
        }
        
        public System.Threading.Tasks.Task<string> UploadFileAsync(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroup document) {
            return base.Channel.UploadFileAsync(document);
        }
        
        public string UploadFileComplex(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroupComplex document) {
            return base.Channel.UploadFileComplex(document);
        }
        
        public System.Threading.Tasks.Task<string> UploadFileComplexAsync(Unito.EUCases.CrawlersUploader.ServiceEUCases.UploadDocumentGroupComplex document) {
            return base.Channel.UploadFileComplexAsync(document);
        }
    }
}