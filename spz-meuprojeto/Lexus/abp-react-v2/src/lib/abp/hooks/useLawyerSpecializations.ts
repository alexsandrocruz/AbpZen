import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLawyerSpecializationsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  lawyerId?: string;
  specializationId?: string;
  }

export function useLawyerSpecializations(input: GetLawyerSpecializationsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["lawyerSpecializations", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer-specialization", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLawyerSpecializations() {
  return useQuery({
    queryKey: ["lawyerSpecializations", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer-specialization", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLawyerSpecialization(id: string) {
  return useQuery({
    queryKey: ["lawyerSpecialization", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lawyer-specialization/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lawyer-specialization", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

export function useUpdateLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lawyer-specialization/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecialization", data.id] });
    },
  });
}

export function useDeleteLawyerSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lawyer-specialization/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: Lawyer <-> Specialization
 * Given a Lawyer, toggle a Specialization
 */
export function useToggleSpecialization(lawyerId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLawyerSpecialization();
  const deleteMutation = useDeleteLawyerSpecialization();
  const { data: existing } = useLawyerSpecializations({ lawyerId: lawyerId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ specializationId, isChecked }: { specializationId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.specializationId === specializationId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          lawyerId: lawyerId,
          specializationId: specializationId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: Lawyer <-> Specialization
 * Given a Specialization, toggle a Lawyer
 */
export function useToggleLawyer(specializationId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLawyerSpecialization();
  const deleteMutation = useDeleteLawyerSpecialization();
  const { data: existing } = useLawyerSpecializations({ specializationId: specializationId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ lawyerId, isChecked }: { lawyerId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.lawyerId === lawyerId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          lawyerId: lawyerId,
          specializationId: specializationId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lawyerSpecializations"] });
    },
  });
}

