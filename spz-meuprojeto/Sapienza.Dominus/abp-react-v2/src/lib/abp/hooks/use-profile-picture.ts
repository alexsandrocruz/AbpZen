import { useState } from "react";
import { toast } from "sonner";

interface UseProfilePictureReturn {
    pictureUrl: string | null;
    isUploading: boolean;
    isDeleting: boolean;
    uploadPicture: (file: File) => Promise<void>;
    deletePicture: () => void;
}

/**
 * Resizes an image file to a maximum dimension while maintaining aspect ratio.
 * Returns a Blob ready for upload.
 */
async function resizeImage(file: File, maxSize: number = 200): Promise<Blob> {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = (e) => {
            const img = new Image();
            img.onload = () => {
                const canvas = document.createElement("canvas");
                let width = img.width;
                let height = img.height;

                // Calculate new dimensions
                if (width > height) {
                    if (width > maxSize) {
                        height = Math.round((height * maxSize) / width);
                        width = maxSize;
                    }
                } else {
                    if (height > maxSize) {
                        width = Math.round((width * maxSize) / height);
                        height = maxSize;
                    }
                }

                canvas.width = width;
                canvas.height = height;

                const ctx = canvas.getContext("2d");
                if (!ctx) {
                    reject(new Error("Could not get canvas context"));
                    return;
                }

                ctx.drawImage(img, 0, 0, width, height);

                canvas.toBlob(
                    (blob) => {
                        if (blob) {
                            resolve(blob);
                        } else {
                            reject(new Error("Could not create blob from canvas"));
                        }
                    },
                    "image/jpeg",
                    0.85
                );
            };
            img.onerror = () => reject(new Error("Failed to load image"));
            img.src = e.target?.result as string;
        };
        reader.onerror = () => reject(new Error("Failed to read file"));
        reader.readAsDataURL(file);
    });
}

/**
 * Hook for managing profile picture upload, display, and deletion.
 * Works locally without backend - displays image immediately after selection.
 */
export function useProfilePicture(): UseProfilePictureReturn {
    const [pictureUrl, setPictureUrl] = useState<string | null>(null);
    const [isUploading, setIsUploading] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);

    const uploadPicture = async (file: File) => {
        if (!file.type.startsWith("image/")) {
            toast.error("Por favor, selecione um arquivo de imagem válido.");
            return;
        }

        setIsUploading(true);
        try {
            // Resize image
            const resizedBlob = await resizeImage(file, 200);

            // Create object URL for immediate local display
            const newUrl = URL.createObjectURL(resizedBlob);

            // Revoke old URL to prevent memory leaks
            if (pictureUrl) {
                URL.revokeObjectURL(pictureUrl);
            }

            setPictureUrl(newUrl);
            toast.success("Foto de perfil atualizada!");

            // TODO: When backend is available, uncomment this:
            // const formData = new FormData();
            // formData.append("ImageContent", resizedBlob, "profile.jpg");
            // await apiClient.post("/api/account/profile-picture", formData, {
            //     headers: { "Content-Type": "multipart/form-data" },
            // });
        } catch (error) {
            console.error("Failed to process profile picture:", error);
            toast.error("Erro ao processar imagem.");
        } finally {
            setIsUploading(false);
        }
    };

    const deletePicture = () => {
        setIsDeleting(true);

        // Revoke URL to prevent memory leaks
        if (pictureUrl) {
            URL.revokeObjectURL(pictureUrl);
        }

        setPictureUrl(null);
        toast.success("Foto de perfil removida!");

        // TODO: When backend is available, uncomment this:
        // await apiClient.delete("/api/account/profile-picture");

        setIsDeleting(false);
    };

    return {
        pictureUrl,
        isUploading,
        isDeleting,
        uploadPicture,
        deletePicture,
    };
}
