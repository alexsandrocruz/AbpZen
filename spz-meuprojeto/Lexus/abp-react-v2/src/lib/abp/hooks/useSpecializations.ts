import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetSpecializationsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useSpecializations(input: GetSpecializationsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["specializations", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/specialization", {
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

export function useAllSpecializations() {
  return useQuery({
    queryKey: ["specializations", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/specialization", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useSpecialization(id: string) {
  return useQuery({
    queryKey: ["specialization", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/specialization/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/specialization", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["specializations"] });
    },
  });
}

export function useUpdateSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/specialization/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["specializations"] });
      queryClient.invalidateQueries({ queryKey: ["specialization", data.id] });
    },
  });
}

export function useDeleteSpecialization() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/specialization/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["specializations"] });
    },
  });
}


