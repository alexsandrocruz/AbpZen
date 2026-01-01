import { X, Copy, Check } from 'lucide-react';
import { useState } from 'react';
import type { ZenMetadata } from '../types';

interface MetadataPreviewProps {
    metadata: ZenMetadata;
    onClose: () => void;
}

export default function MetadataPreview({ metadata, onClose }: MetadataPreviewProps) {
    const [copied, setCopied] = useState(false);
    const jsonString = JSON.stringify(metadata, null, 2);

    const copyToClipboard = () => {
        navigator.clipboard.writeText(jsonString);
        setCopied(true);
        setTimeout(() => setCopied(false), 2000);
    };

    return (
        <div className="modal-overlay">
            <div className="preview-modal">
                <div className="modal-header">
                    <div className="header-left">
                        <h3>Metadata Preview</h3>
                        <span className="badge">JSON</span>
                    </div>
                    <div className="header-actions">
                        <button className="btn-icon-alt" onClick={copyToClipboard}>
                            {copied ? <Check size={18} className="text-green-400" /> : <Copy size={18} />}
                            {copied ? 'Copied!' : 'Copy'}
                        </button>
                        <button className="btn-icon-alt" onClick={onClose}>
                            <X size={18} />
                        </button>
                    </div>
                </div>
                <div className="modal-content">
                    <pre>
                        <code>{jsonString}</code>
                    </pre>
                </div>
            </div>
        </div>
    );
}
