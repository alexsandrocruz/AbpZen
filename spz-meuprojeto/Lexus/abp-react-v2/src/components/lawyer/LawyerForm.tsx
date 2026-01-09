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
import { useCreateLawyer, useUpdateLawyer } from "@/lib/abp/hooks/useLawyers";
import { toast } from "sonner";

const formSchema = z.object({

  fullName: z.any(),

  preferredName: z.any(),

});

type FormValues = z.infer<typeof formSchema>;

interface LawyerFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues?: any;
}

export function LawyerForm({
  isOpen,
  onClose,
  initialValues,
}: LawyerFormProps) {
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

  const createMutation = useCreateLawyer();
  const updateMutation = useUpdateLawyer();

  const onSubmit = async (data: FormValues) => {
    try {
      console.log("Submitting Lawyer:", data);
      if (isEditing) {
        await updateMutation.mutateAsync({ id: initialValues.id, data });
        toast.success("Lawyer updated successfully");
      } else {
        await createMutation.mutateAsync(data);
        toast.success("Lawyer created successfully");
      }
      onClose();
    } catch (error: any) {
      console.error("Failed to save lawyer:", error);
      toast.error(error.message || "Failed to save lawyer");
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Edit Lawyer" : "Create Lawyer"}</DialogTitle>
          <DialogDescription>
            {isEditing ? "Update the details of the lawyer." : "Fill in the details to create a new lawyer."}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-4">

          <div className="space-y-2">
            <Label htmlFor="fullName">FullName</Label>

            <Input id="fullName" {...register("fullName")} />

          </div>

          <div className="space-y-2">
            <Label htmlFor="preferredName">PreferredName</Label>

            <Input id="preferredName" {...register("preferredName")} />

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
