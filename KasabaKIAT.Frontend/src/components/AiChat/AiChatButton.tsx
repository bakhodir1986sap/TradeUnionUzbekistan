import { Button } from 'antd';
import { RobotOutlined } from '@ant-design/icons';

interface Props {
  onClick: () => void;
}

export function AiChatButton({ onClick }: Props) {
  return (
    <Button
      type="primary"
      shape="circle"
      size="large"
      icon={<RobotOutlined />}
      onClick={onClick}
      style={{
        position: 'fixed',
        bottom: 24,
        right: 24,
        zIndex: 1000,
        width: 52,
        height: 52,
        fontSize: 22,
        boxShadow: '0 4px 12px rgba(0,0,0,0.25)',
      }}
    />
  );
}
