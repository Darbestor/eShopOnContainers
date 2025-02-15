// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: IntegrationEvents/Catalog/event_catalog_product_price_changed.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog {

  /// <summary>Holder for reflection information generated from IntegrationEvents/Catalog/event_catalog_product_price_changed.proto</summary>
  public static partial class EventCatalogProductPriceChangedReflection {

    #region Descriptor
    /// <summary>File descriptor for IntegrationEvents/Catalog/event_catalog_product_price_changed.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EventCatalogProductPriceChangedReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CkNJbnRlZ3JhdGlvbkV2ZW50cy9DYXRhbG9nL2V2ZW50X2NhdGFsb2dfcHJv",
            "ZHVjdF9wcmljZV9jaGFuZ2VkLnByb3RvEjVtaWNyb3NvZnQuZXNob3BvbmNv",
            "bnRhaW5lcnMuaW50ZWdyYXRpb25ldmVudHMuY2F0YWxvZxofQ3VzdG9tVHlw",
            "ZXMvZGVjaW1hbF92YWx1ZS5wcm90byLOAQoYUHJvZHVjdFByaWNlQ2hhbmdl",
            "ZFByb3RvEhIKCnByb2R1Y3RfaWQYASABKAUSTgoJbmV3X3ByaWNlGAIgASgL",
            "MjsubWljcm9zb2Z0LmVzaG9wb25jb250YWluZXJzLmludGVncmF0aW9uZXZl",
            "bnRzLkRlY2ltYWxWYWx1ZRJOCglvbGRfcHJpY2UYAyABKAsyOy5taWNyb3Nv",
            "ZnQuZXNob3BvbmNvbnRhaW5lcnMuaW50ZWdyYXRpb25ldmVudHMuRGVjaW1h",
            "bFZhbHVlQlCqAk1NaWNyb3NvZnQuZVNob3BPbkNvbnRhaW5lcnMuU2Vydmlj",
            "ZXMuS2Fma2EuUHJvdG9idWYuSW50ZWdyYXRpb25FdmVudHMuQ2F0YWxvZ2IG",
            "cHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValueReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog.ProductPriceChangedProto), global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog.ProductPriceChangedProto.Parser, new[]{ "ProductId", "NewPrice", "OldPrice" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ProductPriceChangedProto : pb::IMessage<ProductPriceChangedProto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<ProductPriceChangedProto> _parser = new pb::MessageParser<ProductPriceChangedProto>(() => new ProductPriceChangedProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<ProductPriceChangedProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Catalog.EventCatalogProductPriceChangedReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProductPriceChangedProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProductPriceChangedProto(ProductPriceChangedProto other) : this() {
      productId_ = other.productId_;
      newPrice_ = other.newPrice_ != null ? other.newPrice_.Clone() : null;
      oldPrice_ = other.oldPrice_ != null ? other.oldPrice_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public ProductPriceChangedProto Clone() {
      return new ProductPriceChangedProto(this);
    }

    /// <summary>Field number for the "product_id" field.</summary>
    public const int ProductIdFieldNumber = 1;
    private int productId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int ProductId {
      get { return productId_; }
      set {
        productId_ = value;
      }
    }

    /// <summary>Field number for the "new_price" field.</summary>
    public const int NewPriceFieldNumber = 2;
    private global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue newPrice_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue NewPrice {
      get { return newPrice_; }
      set {
        newPrice_ = value;
      }
    }

    /// <summary>Field number for the "old_price" field.</summary>
    public const int OldPriceFieldNumber = 3;
    private global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue oldPrice_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue OldPrice {
      get { return oldPrice_; }
      set {
        oldPrice_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as ProductPriceChangedProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(ProductPriceChangedProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ProductId != other.ProductId) return false;
      if (!object.Equals(NewPrice, other.NewPrice)) return false;
      if (!object.Equals(OldPrice, other.OldPrice)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (ProductId != 0) hash ^= ProductId.GetHashCode();
      if (newPrice_ != null) hash ^= NewPrice.GetHashCode();
      if (oldPrice_ != null) hash ^= OldPrice.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (ProductId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ProductId);
      }
      if (newPrice_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(NewPrice);
      }
      if (oldPrice_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(OldPrice);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (ProductId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(ProductId);
      }
      if (newPrice_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(NewPrice);
      }
      if (oldPrice_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(OldPrice);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (ProductId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(ProductId);
      }
      if (newPrice_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(NewPrice);
      }
      if (oldPrice_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(OldPrice);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(ProductPriceChangedProto other) {
      if (other == null) {
        return;
      }
      if (other.ProductId != 0) {
        ProductId = other.ProductId;
      }
      if (other.newPrice_ != null) {
        if (newPrice_ == null) {
          NewPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
        }
        NewPrice.MergeFrom(other.NewPrice);
      }
      if (other.oldPrice_ != null) {
        if (oldPrice_ == null) {
          OldPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
        }
        OldPrice.MergeFrom(other.OldPrice);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            ProductId = input.ReadInt32();
            break;
          }
          case 18: {
            if (newPrice_ == null) {
              NewPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
            }
            input.ReadMessage(NewPrice);
            break;
          }
          case 26: {
            if (oldPrice_ == null) {
              OldPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
            }
            input.ReadMessage(OldPrice);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            ProductId = input.ReadInt32();
            break;
          }
          case 18: {
            if (newPrice_ == null) {
              NewPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
            }
            input.ReadMessage(NewPrice);
            break;
          }
          case 26: {
            if (oldPrice_ == null) {
              OldPrice = new global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.DecimalValue();
            }
            input.ReadMessage(OldPrice);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
