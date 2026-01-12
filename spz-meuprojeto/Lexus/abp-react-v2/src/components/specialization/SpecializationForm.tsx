import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Loader2 } from "lucide-react";
import { useEffect } from "react";
import { useCreateSpecialization, useUpdateSpecialization } from "@/lib/abp/hooks/useSpecializations";
import { toast } from "sonner";

const formSchema = z.object({
  
  name: z.string(),
  
  description: z.string().optional(),
  
});

type FormValues = z.infer<typeof formSchema>;

interface SpecializationFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues?: any;
}

export function SpecializationForm({
  isOpen,
  onClose,
  initialValues,
}: SpecializationFormProps) {
  const isEditing = !!initialValues;
  
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateSpecialization();
  const updateMutation = useUpdateSpecialization();

  const onSubmit = async (data: FormValues) => {
    try {
      if (isEditing) {
        await updateMutation.mutateAsync({ id: initialValues.id, data });
        toast.success("Specialization updated successfully");
      } else {
        await createMutation.mutateAsync(data);
        toast.success("Specialization created successfully");
      }
      onClose();
    } catch (error: any) {
      console.error("Failed to save specialization:", error);
      toast.error(error.message || "Failed to save specialization");
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Edit Specialization" : "Create Specialization"}</DialogTitle>
          <DialogDescription>
            {isEditing ? "Update the details of the specialization." : "Fill in the details to create a new specialization."}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-4">
          
          <div className="space-y-2">
            <Label htmlFor="name">Name *</Label>
            
            <Input id="name" {...register("name")} />
            
          </div>
          
          <div className="space-y-2">
            <Label htmlFor="description">Description</Label>
            
            <Input id="description" {...register("description")} />
            
          </div>
          

          <DialogFooter>
            <Button type="button" variant="outline" onClick={onClose} disabled={isSubmitting}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isEditing ? "Save Changes" : "Create"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
