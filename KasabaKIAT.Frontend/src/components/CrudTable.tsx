import { useState, type ReactNode } from 'react';
import { Button, Table, Space, Modal, Form, App, Typography } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import type { ColumnType } from 'antd/es/table';
import { QueryErrorAlert } from './QueryErrorAlert';

interface CrudTableProps<T extends { id: number }> {
  title: string;
  columns: ColumnType<T>[];
  data: T[];
  isLoading: boolean;
  isError: boolean;
  onRetry: () => void;
  isSaving: boolean;
  isDeleting?: boolean;
  onSave: (values: Record<string, unknown>, editing: T | null) => Promise<void> | void;
  onDelete: (id: number) => void;
  deleteConfirmTitle?: string;
  formFields: ReactNode;
  modalWidth?: number;
  pageSize?: number;
  extraHeaderContent?: ReactNode;
}

export function CrudTable<T extends { id: number }>({
  title,
  columns,
  data,
  isLoading,
  isError,
  onRetry,
  isSaving,
  isDeleting,
  onSave,
  onDelete,
  deleteConfirmTitle = 'Удалить запись?',
  formFields,
  modalWidth,
  pageSize = 20,
  extraHeaderContent,
}: CrudTableProps<T>) {
  const { modal } = App.useApp();
  const [form] = Form.useForm();
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<T | null>(null);

  const openAdd = () => {
    setEditing(null);
    form.resetFields();
    setModalOpen(true);
  };

  const openEdit = (rec: T) => {
    setEditing(rec);
    form.setFieldsValue(rec);
    setModalOpen(true);
  };

  const handleCancel = () => {
    setModalOpen(false);
  };

  const confirmDelete = (id: number) =>
    modal.confirm({ title: deleteConfirmTitle, onOk: () => onDelete(id) });

  const actionColumn: ColumnType<T> = {
    title: 'Действия',
    key: '_actions',
    width: 100,
    render: (_: unknown, rec: T) => (
      <Space>
        <Button size="small" icon={<EditOutlined />} onClick={() => openEdit(rec)} />
        <Button size="small" danger icon={<DeleteOutlined />} loading={isDeleting} onClick={() => confirmDelete(rec.id)} />
      </Space>
    ),
  };

  const allColumns = [...columns, actionColumn];

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: 16 }}>
        <Typography.Title level={4} style={{ margin: 0 }}>{title}</Typography.Title>
        <Space>
          {extraHeaderContent}
          <Button type="primary" icon={<PlusOutlined />} onClick={openAdd}>Добавить</Button>
        </Space>
      </div>

      <QueryErrorAlert isError={isError} onRetry={onRetry} />
      <Table<T>
        columns={allColumns}
        dataSource={data}
        rowKey="id"
        loading={isLoading}
        size="small"
        pagination={{ pageSize }}
      />

      <Modal
        title={editing ? 'Редактировать' : 'Добавить'}
        open={modalOpen}
        onOk={() => form.submit()}
        onCancel={handleCancel}
        confirmLoading={isSaving}
        width={modalWidth}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form
          form={form}
          layout="vertical"
          onFinish={async (v) => {
            await onSave(v as Record<string, unknown>, editing);
            setModalOpen(false);
          }}
        >
          {formFields}
        </Form>
      </Modal>
    </div>
  );
}
