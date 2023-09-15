// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: IntegrationEvents/Ordering/event_ordering_status_changed_to_awaiting.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering {

  /// <summary>Holder for reflection information generated from IntegrationEvents/Ordering/event_ordering_status_changed_to_awaiting.proto</summary>
  public static partial class EventOrderingStatusChangedToAwaitingReflection {

    #region Descriptor
    /// <summary>File descriptor for IntegrationEvents/Ordering/event_ordering_status_changed_to_awaiting.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EventOrderingStatusChangedToAwaitingReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CkpJbnRlZ3JhdGlvbkV2ZW50cy9PcmRlcmluZy9ldmVudF9vcmRlcmluZ19z",
            "dGF0dXNfY2hhbmdlZF90b19hd2FpdGluZy5wcm90bxI2bWljcm9zb2Z0LmVz",
            "aG9wb25jb250YWluZXJzLmludGVncmF0aW9uZXZlbnRzLm9yZGVyaW5nGjZJ",
            "bnRlZ3JhdGlvbkV2ZW50cy9PcmRlcmluZy9ldmVudF9vcmRlcmluZ19jb21t",
            "b24ucHJvdG8itwEKO09yZGVyU3RhdHVzQ2hhbmdlZFRvQXdhaXRpbmdWYWxp",
            "ZGF0aW9uSW50ZWdyYXRpb25FdmVudFByb3RvEhAKCG9yZGVyX2lkGAEgASgF",
            "EmYKEW9yZGVyX3N0b2NrX2l0ZW1zGAIgAygLMksubWljcm9zb2Z0LmVzaG9w",
            "b25jb250YWluZXJzLmludGVncmF0aW9uZXZlbnRzLm9yZGVyaW5nLk9yZGVy",
            "U3RvY2tJdGVtUHJvdG9CUaoCTk1pY3Jvc29mdC5lU2hvcE9uQ29udGFpbmVy",
            "cy5TZXJ2aWNlcy5LYWZrYS5Qcm90b2J1Zi5JbnRlZ3JhdGlvbkV2ZW50cy5P",
            "cmRlcmluZ2IGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.EventOrderingCommonReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStatusChangedToAwaitingValidationIntegrationEventProto), global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStatusChangedToAwaitingValidationIntegrationEventProto.Parser, new[]{ "OrderId", "OrderStockItems" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class OrderStatusChangedToAwaitingValidationIntegrationEventProto : pb::IMessage<OrderStatusChangedToAwaitingValidationIntegrationEventProto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<OrderStatusChangedToAwaitingValidationIntegrationEventProto> _parser = new pb::MessageParser<OrderStatusChangedToAwaitingValidationIntegrationEventProto>(() => new OrderStatusChangedToAwaitingValidationIntegrationEventProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<OrderStatusChangedToAwaitingValidationIntegrationEventProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.EventOrderingStatusChangedToAwaitingReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OrderStatusChangedToAwaitingValidationIntegrationEventProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OrderStatusChangedToAwaitingValidationIntegrationEventProto(OrderStatusChangedToAwaitingValidationIntegrationEventProto other) : this() {
      orderId_ = other.orderId_;
      orderStockItems_ = other.orderStockItems_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OrderStatusChangedToAwaitingValidationIntegrationEventProto Clone() {
      return new OrderStatusChangedToAwaitingValidationIntegrationEventProto(this);
    }

    /// <summary>Field number for the "order_id" field.</summary>
    public const int OrderIdFieldNumber = 1;
    private int orderId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int OrderId {
      get { return orderId_; }
      set {
        orderId_ = value;
      }
    }

    /// <summary>Field number for the "order_stock_items" field.</summary>
    public const int OrderStockItemsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStockItemProto> _repeated_orderStockItems_codec
        = pb::FieldCodec.ForMessage(18, global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStockItemProto.Parser);
    private readonly pbc::RepeatedField<global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStockItemProto> orderStockItems_ = new pbc::RepeatedField<global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStockItemProto>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Microsoft.eShopOnContainers.Services.Kafka.Protobuf.IntegrationEvents.Ordering.OrderStockItemProto> OrderStockItems {
      get { return orderStockItems_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as OrderStatusChangedToAwaitingValidationIntegrationEventProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(OrderStatusChangedToAwaitingValidationIntegrationEventProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (OrderId != other.OrderId) return false;
      if(!orderStockItems_.Equals(other.orderStockItems_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (OrderId != 0) hash ^= OrderId.GetHashCode();
      hash ^= orderStockItems_.GetHashCode();
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
      if (OrderId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(OrderId);
      }
      orderStockItems_.WriteTo(output, _repeated_orderStockItems_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (OrderId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(OrderId);
      }
      orderStockItems_.WriteTo(ref output, _repeated_orderStockItems_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (OrderId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(OrderId);
      }
      size += orderStockItems_.CalculateSize(_repeated_orderStockItems_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(OrderStatusChangedToAwaitingValidationIntegrationEventProto other) {
      if (other == null) {
        return;
      }
      if (other.OrderId != 0) {
        OrderId = other.OrderId;
      }
      orderStockItems_.Add(other.orderStockItems_);
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
            OrderId = input.ReadInt32();
            break;
          }
          case 18: {
            orderStockItems_.AddEntriesFrom(input, _repeated_orderStockItems_codec);
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
            OrderId = input.ReadInt32();
            break;
          }
          case 18: {
            orderStockItems_.AddEntriesFrom(ref input, _repeated_orderStockItems_codec);
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
